using GreenLoop.BLL.DTOs;
using GreenLoop.DAL.Entities;

namespace GreenLoop.BLL.Interfaces.IServices
{
    public interface IQRService
    {
        Task<ScanResponseDto> ProcessScanAsync(ScanRequestDto request, int driverId);
        Task<RedeemResponseDto> RedeemCouponAsync(RedeemRequestDto request, int customerId);
        Task<QrTokenDto> GenerateQrTokenAsync(int customerId);
    }
}
