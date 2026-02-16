using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Enums;

namespace GreenLoop.DAL.Interfaces.IRepositories
{
    public interface IDriverRepository
    {
        Task<Driver?> GetDriverByIdAsync(int driverId); 
        Task<List<PickupRequest>> GetDriverTasksAsync(int driverId, DateTime date, RequestStatus? status = null);
        Task<PickupRequest?> GetTaskByIdAsync(int taskId);
        Task<List<WasteCategory>> GetWasteCategoriesByIdsAsync(List<int> ids);

        Task<bool> SaveChangesAsync();
    }
}
