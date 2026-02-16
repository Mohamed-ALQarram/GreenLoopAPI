using GreenLoop.BLL.DTOs;

namespace GreenLoop.BLL.Interfaces
{
    public interface IWalletService
    {
        Task<WalletBalanceDto> GetBalanceAsync(int customerId);
        Task<List<CouponDto>> GetAvailableCouponsAsync(int customerId);
        Task<RedeemResponseDto?> RedeemCouponAsync(int customerId, RedeemRequestDto dto);
        Task<List<TransactionDto>> GetTransactionsAsync(int customerId, int page, int pageSize);
    }
}
