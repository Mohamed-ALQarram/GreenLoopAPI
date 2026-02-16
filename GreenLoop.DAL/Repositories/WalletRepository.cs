using GreenLoop.DAL.Data;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GreenLoop.DAL.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly GreenLoopDbContext _context;

        public WalletRepository(GreenLoopDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerWithWalletAsync(int customerId)
        {
            return await _context.Users.OfType<Customer>()
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<List<Coupon>> GetAvailableCouponsAsync()
        {
            return await _context.Coupons
                .Include(c => c.Business)
                .Where(c => c.IsActive && (c.ExpiryDate == null || c.ExpiryDate > DateTime.UtcNow))
                .ToListAsync();
        }

        public async Task<Coupon?> GetCouponByIdAsync(Guid couponId)
        {
             return await _context.Coupons.FindAsync(couponId);
        }

        public async Task AddTransactionAsync(WalletTransaction transaction)
        {
            await _context.Set<WalletTransaction>().AddAsync(transaction);
        }

        public async Task AddUserCouponAsync(UserCoupon userCoupon)
        {
             await _context.Set<UserCoupon>().AddAsync(userCoupon);
        }

        public async Task<List<WalletTransaction>> GetTransactionsAsync(int customerId, int pageNumber, int pageSize)
        {
            return await _context.Set<WalletTransaction>()
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
