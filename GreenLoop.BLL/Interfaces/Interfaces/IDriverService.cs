using GreenLoop.BLL.DTOs;

namespace GreenLoop.BLL.Interfaces
{
    public interface IDriverService
    {
        Task<bool> UpdateStatusAsync(int driverId, string status);
        Task<List<DriverTaskDto>> GetAssignedTasksAsync(int driverId, DateTime date);
        Task<bool> StartTaskAsync(int taskId);
        Task<decimal?> CompleteTaskAsync(int taskId, TaskCompleteDto dto); // Returns points, nullable if failed
    }
}
