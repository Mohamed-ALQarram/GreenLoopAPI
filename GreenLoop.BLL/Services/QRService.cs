using GreenLoop.BLL.DTOs;
using GreenLoop.BLL.Interfaces.IServices;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Interfaces.IRepositories;
using System;
using System.Threading.Tasks;

namespace GreenLoop.BLL.Services
{
    public class QRService : IQRService
    {
        private readonly IPointTransactionRepository _pointTransactionRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly string _secretKey = "GreenLoop_Secret_Key_Change_Me_In_Prod_!@#123"; // Should be in config

        public QRService(IPointTransactionRepository pointTransactionRepository, IWalletRepository walletRepository)
        {
            _pointTransactionRepository = pointTransactionRepository;
            _walletRepository = walletRepository;
        }

        public async Task<ScanResponseDto> ProcessScanAsync(ScanRequestDto request, int driverId)
        {
            int customerId;
            // 1. Try to parse as simple ID (Legacy/Simple Mode)
            if (int.TryParse(request.QrCode, out int simpleId))
            {
                customerId = simpleId;
            }
            else
            {
                // 2. Try to parse as Secure Token
                customerId = ValidateQrToken(request.QrCode);
            }

            // 3. Validate Customer exists
            var customer = await _walletRepository.GetCustomerWithWalletAsync(customerId);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            // 4. Prevent duplicate scan within 5 minutes
            bool hasScanned = await _pointTransactionRepository.HasScannedRecentlyAsync(customerId, request.QrCode, 5);
            if (hasScanned)
            {
                throw new InvalidOperationException("Duplicate scan detected. Please wait 5 minutes.");
            }

            // 5. Add fixed 10 points to customer
            int pointsToAdd = 10;
            customer.PointsBalance += pointsToAdd;
            customer.TotalPointsEarned += pointsToAdd;

            // 6. Create PointTransaction record
            var transaction = new PointTransaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                DriverId = driverId,
                PointsAdded = pointsToAdd,
                QrCodeValue = request.QrCode, // Store what was actually scanned (token or id)
                CreatedAt = DateTime.UtcNow
            };

            await _pointTransactionRepository.AddAsync(transaction);
            await _walletRepository.SaveChangesAsync();

            return new ScanResponseDto
            {
                CustomerId = customerId,
                PointsAdded = pointsToAdd,
                NewTotalPoints = customer.PointsBalance,
                Message = "Points added successfully."
            };
        }

        public async Task<RedeemResponseDto> RedeemCouponAsync(RedeemRequestDto request, int customerId)
        {
            // 1. Get authenticated Customer
            var customer = await _walletRepository.GetCustomerWithWalletAsync(customerId);
            if (customer == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

            // 2. Get Coupon
            var coupon = await _walletRepository.GetCouponByIdAsync(request.CouponId);
            if (coupon == null)
            {
                throw new KeyNotFoundException("Coupon not found.");
            }

            if (!coupon.IsActive || (coupon.ExpiryDate.HasValue && coupon.ExpiryDate < DateTime.UtcNow))
            {
                throw new InvalidOperationException("Coupon is not active or has expired.");
            }

            // 3. Check if customer has enough points
            if (customer.PointsBalance < coupon.RequiredPoints)
            {
                throw new InvalidOperationException("Insufficient points.");
            }

            // 4. Subtract required points
            customer.PointsBalance -= coupon.RequiredPoints;

            // 5. Create UserCoupon
            var userCoupon = new UserCoupon
            {
                CustomerId = customerId,
                CouponId = coupon.Id,
                Code = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
                RedeemedDate = DateTime.UtcNow,
                IsUsed = false
            };
            
            await _walletRepository.AddUserCouponAsync(userCoupon);

            // 6. Save changes
            await _walletRepository.SaveChangesAsync();

            return new RedeemResponseDto
            {
                CouponCode = userCoupon.Code,
                RemainingBalance = customer.PointsBalance
            };
        }

        public Task<QrTokenDto> GenerateQrTokenAsync(int customerId)
        {
            var expiry = DateTime.UtcNow.AddMinutes(5); // Valid for 5 minutes
            var payload = $"{customerId}|{expiry.Ticks}|{Guid.NewGuid()}";
            var signature = ComputeHmacSha256(payload, _secretKey);
            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{payload}.{signature}"));

            return Task.FromResult(new QrTokenDto
            {
                Token = token,
                ExpiresAt = expiry
            });
        }

        private int ValidateQrToken(string token)
        {
            try
            {
                var decodedValues = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split('.');
                if (decodedValues.Length != 2) throw new Exception();

                var payload = decodedValues[0];
                var providedSignature = decodedValues[1];

                var expectedSignature = ComputeHmacSha256(payload, _secretKey);
                if (providedSignature != expectedSignature) throw new ArgumentException("Invalid QR Token signature.");

                var parts = payload.Split('|');
                int customerId = int.Parse(parts[0]);
                long expiryTicks = long.Parse(parts[1]);

                if (new DateTime(expiryTicks) < DateTime.UtcNow)
                {
                     throw new InvalidOperationException("QR Code has expired.");
                }

                return customerId;
            }
            catch (FormatException)
            {
                 throw new ArgumentException("Invalid QR Code format.");
            }
            catch (IndexOutOfRangeException)
            {
                 throw new ArgumentException("Invalid QR Code format.");
            }
        }

        private string ComputeHmacSha256(string data, string key)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
