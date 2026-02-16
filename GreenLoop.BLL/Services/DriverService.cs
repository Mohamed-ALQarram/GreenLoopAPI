using GreenLoop.BLL.DTOs;
using GreenLoop.BLL.Interfaces;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Enums;
using GreenLoop.DAL.Interfaces.IRepositories;

namespace GreenLoop.BLL.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _repository;

        public DriverService(IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> UpdateStatusAsync(int driverId, string status)
        {
            if (Enum.TryParse<DriverStatus>(status, true, out var driverStatus))
            {
                var driver = await _repository.GetDriverByIdAsync(driverId);
                if (driver == null) return false;
                
                driver.Status = driverStatus;
                return await _repository.SaveChangesAsync();
            }
            return false;
        }

        public async Task<List<DriverTaskDto>> GetAssignedTasksAsync(int driverId, DateTime date)
        {
             var tasks = await _repository.GetDriverTasksAsync(driverId, date, RequestStatus.Assigned);
             
             return tasks.Select(t => new DriverTaskDto
             {
                 Id = t.Id,
                 Status = t.Status.ToString(),
                 ScheduledDate = t.ScheduledDate,
                 CustomerName = t.Customer?.FullName ?? "Unknown",
                 Address = $"{t.Address?.City}, {t.Address?.StreetName}" 
             }).ToList();
        }

        public async Task<bool> StartTaskAsync(int taskId)
        {
            var task = await _repository.GetTaskByIdAsync(taskId);
            if (task == null) return false;
            
            task.Status = RequestStatus.InProgress;
            task.StartTime = DateTime.UtcNow;
            
            return await _repository.SaveChangesAsync();
        }

        public async Task<decimal?> CompleteTaskAsync(int taskId, TaskCompleteDto dto)
        {
            var task = await _repository.GetTaskByIdAsync(taskId);
            if (task == null) return null;

            task.Status = RequestStatus.Completed;
            task.CompletedDate = DateTime.UtcNow;


            // Handle Waste Types
            var categories = await _repository.GetWasteCategoriesByIdsAsync(dto.WasteCategoryIds);

            // Logic: I will create/update RequestDetails. 
            // Since the user might send new types, I should perhaps add them.
            // Simplified: If there are no details, create them. If there are, maybe just log the weight?
            // The prompt implies we have "Actual Weight" (Total) and "Waste Types".
            // Since RequestDetail has ActualWeight per Category, I will distribute the total weight evenly 
            // OR mostly likely just assign it to the first category found, or create a detail for each category with 0 weight except first?
            // Let's assume we split weight evenly for now or just put it on the first one.
            // Better: If categories are found, create details for them. 

            if (categories.Any())
            {
                // Clear existing if any? Or append?
                // "Stores actual weight, Stores waste types".
                // I'll update the details list.
                
                // If weight is total, I need to split? 
                decimal weightPerCategory = dto.ActualWeight / categories.Count;

                foreach (var cat in categories)
                {
                    var existingDetail = task.Details.FirstOrDefault(d => d.CategoryId == cat.Id);
                    if (existingDetail != null)
                    {
                        existingDetail.ActualWeight = weightPerCategory;
                    }
                    else
                    {
                        task.Details.Add(new RequestDetail
                        {
                            CategoryId = cat.Id,
                            Category = cat, // EF core tracking might handle this
                            ActualWeight = weightPerCategory,
                            EstimatedWeight = 0, // Unknown
                            PointsEarned = 0 // Calculated later
                        });
                    }
                }
            }
            
            // Calculate points: ActualWeight * 0.5
            decimal points = dto.ActualWeight * 0.5m;
            task.TotalPointsEarned = (int)points; // Casting to int as per Entity definition

            await _repository.SaveChangesAsync();
            return points;
        }
    }
}
