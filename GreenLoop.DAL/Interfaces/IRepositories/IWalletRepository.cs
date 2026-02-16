using GreenLoop.DAL.Entities;

namespace GreenLoop.DAL.Interfaces.IRepositories
{
    public interface IWalletRepository
    {
        Task<Customer?> GetCustomerWithWalletAsync(int customerId);
        Task<List<Coupon>> GetAvailableCouponsAsync();
        Task<Coupon?> GetCouponByIdAsync(int couponId);
        Task AddTransactionAsync(WalletTransaction transaction);
        Task AddUserCouponAsync(UserCoupon userCoupon);
        Task<List<WalletTransaction>> GetTransactionsAsync(int customerId, int pageNumber, int pageSize);
        Task<bool> SaveChangesAsync();
    }
}
