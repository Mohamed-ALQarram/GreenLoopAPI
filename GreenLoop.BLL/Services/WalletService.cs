using GreenLoop.BLL.DTOs;
using GreenLoop.BLL.Interfaces;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Enums;
using GreenLoop.DAL.Interfaces.IRepositories;
using System.Text;

namespace GreenLoop.BLL.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _repository;

        public WalletService(IWalletRepository repository)
        {
            _repository = repository;
        }

        public async Task<WalletBalanceDto> GetBalanceAsync(int customerId)
        {
            var customer = await _repository.GetCustomerWithWalletAsync(customerId);
            if (customer == null) return new WalletBalanceDto { CurrentBalance = 0, TotalEarned = 0 };

            return new WalletBalanceDto
            {
                CurrentBalance = customer.PointsBalance,
                TotalEarned = customer.TotalPointsEarned
            };
        }

        public async Task<List<CouponDto>> GetAvailableCouponsAsync(int customerId)
        {
            var coupons = await _repository.GetAvailableCouponsAsync();
            return coupons.Select(c => new CouponDto
            {
                CouponId = c.Id,
                Title = c.Title,
                PartnerName = c.Business?.BusinessName ?? "Market", // Assuming active coupons have businesses loaded
                RequiredPoints = c.RequiredPoints,
                ImageUrl = c.ImageUrl
            }).ToList();
        }

        public async Task<RedeemResponseDto?> RedeemCouponAsync(int customerId, RedeemRequestDto dto)
        {
            var customer = await _repository.GetCustomerWithWalletAsync(customerId);
            if (customer == null) return null; // Customer not found

            var coupon = await _repository.GetCouponByIdAsync(dto.CouponId);
            if (coupon == null || !coupon.IsActive) return null; // Invalid or inactive coupon
            if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate < DateTime.UtcNow) return null;

            if (customer.PointsBalance < coupon.RequiredPoints) return null; // Insufficient balance

            // Deduct points
            customer.PointsBalance -= coupon.RequiredPoints;

            // Generate Code
            string code = GenerateCouponCode(coupon.Id, customerId);

            // Record Transaction
            var transaction = new WalletTransaction
            {
                CustomerId = customerId,
                Amount = -coupon.RequiredPoints,
                Type = TransactionType.Redeemed,
                Date = DateTime.UtcNow,
                Description = $"Redeemed coupon: {coupon.Title}"
            };

            var userCoupon = new UserCoupon
            {
                CustomerId = customerId,
                CouponId = coupon.Id,
                Code = code,
                RedeemedDate = DateTime.UtcNow,
                IsUsed = false
            };

            await _repository.AddTransactionAsync(transaction);
            await _repository.AddUserCouponAsync(userCoupon);
            await _repository.SaveChangesAsync();

            return new RedeemResponseDto
            {
                CouponCode = code,
                RemainingBalance = customer.PointsBalance
            };
        }

        public async Task<List<TransactionDto>> GetTransactionsAsync(int customerId, int page, int pageSize)
        {
            var transactions = await _repository.GetTransactionsAsync(customerId, page, pageSize);
            return transactions.Select(t => new TransactionDto
            {
                Type = t.Type.ToString(),
                Points = t.Amount,
                Description = t.Description ?? "",
                Date = t.Date
            }).ToList();
        }

        private string GenerateCouponCode(Guid couponId, int customerId)
        {
             // Simple unique code generation
             return $"CPN-{couponId}-{customerId}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
