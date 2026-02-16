using GreenLoop.DAL.Interfaces;
using GreenLoop.DAL.Data;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace GreenLoop.DAL.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly GreenLoopDbContext _context;

        public RequestRepository(GreenLoopDbContext context)
        {
            _context = context;
        }

        public async Task<List<WasteCategory>> GetAllWasteCategoriesAsync()
        {
            return await _context.WasteCategories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(int userId)
        {
            return await _context.Users
                .OfType<Customer>().Include(x=>x.Addresses)
                .FirstOrDefaultAsync(c => c.Id == userId);
        }

        public async Task<PickupRequest> AddPickupRequestAsync(PickupRequest request)
        {
            await _context.PickupRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<List<PickupRequest>> GetRequestsByCustomerIdAsync(
            int customerId, int page, int pageSize, RequestStatus? status)
        {
            var query = _context.PickupRequests
                .AsNoTracking()
                .Include(r => r.Address)
                .Where(r => r.CustomerId == customerId);

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            return await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetRequestCountByCustomerIdAsync(int customerId, RequestStatus? status)
        {
            var query = _context.PickupRequests
                .AsNoTracking()
                .Where(r => r.CustomerId == customerId);

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            return await query.CountAsync();
        }

        public async Task<PickupRequest?> GetRequestByIdAsync(int id)
        {
            return await _context.PickupRequests
                .Include(r => r.Address)
                .Include(r => r.Details)
                    .ThenInclude(d => d.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateRequestAsync(PickupRequest request)
        {
            _context.PickupRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task<WasteCategory> AddWasteCategoryAsync(WasteCategory category)
        {
            await _context.WasteCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
