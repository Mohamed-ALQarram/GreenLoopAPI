using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Enums;

namespace GreenLoop.DAL.Interfaces
{
    public interface IRequestRepository
    {
        Task<List<WasteCategory>> GetAllWasteCategoriesAsync();
        Task<Customer?> GetCustomerByUserIdAsync(int userId);
        Task<PickupRequest> AddPickupRequestAsync(PickupRequest request);
        Task<List<PickupRequest>> GetRequestsByCustomerIdAsync(int customerId, int page, int pageSize, RequestStatus? status);
        Task<int> GetRequestCountByCustomerIdAsync(int customerId, RequestStatus? status);
        Task<PickupRequest?> GetRequestByIdAsync(int id);
        Task UpdateRequestAsync(PickupRequest request);
        Task<WasteCategory> AddWasteCategoryAsync(WasteCategory category);
    }
}
