using GreenLoop.BLL.DTOs;
using GreenLoop.DAL.Enums;

namespace GreenLoop.BLL.Interfaces.IServices
{
    public interface IRequestService
    {
        Task<List<WasteCategoryDto>> GetWasteCategoriesAsync();
        Task<WasteCategoryDto> AddWasteCategoryAsync(CreateWasteCategoryDto dto);
        Task<int> SchedulePickupAsync(int userId, SchedulePickupRequestDto dto);
        Task<PagedResult<RequestHistoryDto>> GetRequestHistoryAsync(int userId, int page, RequestStatus? status);
        Task<RequestDetailsResponseDto?> GetRequestDetailsAsync(int userId, int requestId);
        Task<bool> CancelRequestAsync(int userId, int requestId);
    }
}
