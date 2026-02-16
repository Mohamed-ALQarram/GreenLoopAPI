using GreenLoop.DAL.Data;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GreenLoop.DAL.Repositories
{
    public class PointTransactionRepository : IPointTransactionRepository
    {
        private readonly GreenLoopDbContext _context;

        public PointTransactionRepository(GreenLoopDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PointTransaction transaction)
        {
            await _context.PointTransactions.AddAsync(transaction);
        }

        public async Task<PointTransaction?> GetLatestTransactionAsync(int customerId, int driverId)
        {
            return await _context.PointTransactions
                .Where(t => t.CustomerId == customerId && t.DriverId == driverId)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasScannedRecentlyAsync(int customerId, string qrCodeValue, int minutes)
        {
             var threshold = DateTime.UtcNow.AddMinutes(-minutes);
             return await _context.PointTransactions
                 .AnyAsync(t => t.CustomerId == customerId && 
                                t.QrCodeValue == qrCodeValue && 
                                t.CreatedAt > threshold);
        }
    }
}
