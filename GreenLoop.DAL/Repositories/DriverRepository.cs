using GreenLoop.DAL.Data;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Enums;
using GreenLoop.DAL.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace GreenLoop.DAL.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly GreenLoopDbContext _context;

        public DriverRepository(GreenLoopDbContext context)
        {
            _context = context;
        }

        public async Task<Driver?> GetDriverByIdAsync(int driverId)
        {
            return await _context.Users.OfType<Driver>().FirstOrDefaultAsync(d => d.Id == driverId);
        }

        public async Task<List<PickupRequest>> GetDriverTasksAsync(int driverId, DateTime date, RequestStatus? status = null)
        {
             var query = _context.PickupRequests
                .Include(t => t.Customer)
                .Include(t => t.Address)
                .Where(t => t.DriverId == driverId && t.ScheduledDate.Date == date.Date);

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            // Tasks should be ordered by route/priority.
            return await query.OrderBy(t => t.ScheduledDate).ToListAsync();
        }

        public async Task<PickupRequest?> GetTaskByIdAsync(int taskId)
        {
             return await _context.PickupRequests
                 .Include(t=>t.Driver)
                 .Include(t => t.Details) // Include details to update them if needed
                 .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<List<WasteCategory>> GetWasteCategoriesByIdsAsync(List<int> ids)
        {
             return await _context.WasteCategories
                .Where(c => ids.Contains(c.Id)) 
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
