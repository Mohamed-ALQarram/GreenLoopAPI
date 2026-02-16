using GreenLoop.BLL.DTOs;
using GreenLoop.BLL.Interfaces.IServices;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Enums;
using GreenLoop.DAL.Interfaces;
using GreenLoop.DAL.Interfaces.IRepositories;
using GreenLoop.DAL.Repositories;

namespace GreenLoop.BLL.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private const int DefaultPageSize = 10;

        public RequestService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        // ─── GET Waste Categories ───────────────────────────────────────
        public async Task<List<WasteCategoryDto>> GetWasteCategoriesAsync()
        {
            var categories = await _requestRepository.GetAllWasteCategoriesAsync();

            return categories.Select(c => new WasteCategoryDto
            {
                Id = c.Id,
                NameAr = c.NameAr,
                IconUrl = c.IconUrl
            }).ToList();
        }

        // ─── Add Waste Category ─────────────────────────────────────────
        public async Task<WasteCategoryDto> AddWasteCategoryAsync(CreateWasteCategoryDto dto)
        {
            var category = new WasteCategory
            {
                NameAr = dto.NameAr,
                PointsPerKg = dto.PointsPerKg,
                MarketPricePerTon = dto.MarketPricePerTon,
                IconUrl = dto.IconUrl
            };

            var created = await _requestRepository.AddWasteCategoryAsync(category);

            return new WasteCategoryDto
            {
                Id = created.Id,
                NameAr = created.NameAr,
                IconUrl = created.IconUrl
            };
        }

        // ─── Schedule Pickup Request ────────────────────────────────────
        public async Task<int> SchedulePickupAsync(int userId, SchedulePickupRequestDto dto)
        {
            // 1. Validate customer exists (Addresses are included)
            var customer = await _requestRepository.GetCustomerByUserIdAsync(userId);
            if (customer == null)
                throw new InvalidOperationException("Customer not found.");

            // 2. Find existing address: prefer default, fallback to first
            var address = customer.Addresses.FirstOrDefault(a => a.IsDefault)
                       ?? customer.Addresses.FirstOrDefault();

            PickupRequest pickupRequest;

            if (address != null)
            {
                // 3a. Update Lat/Long on the existing address
                address.Latitude = dto.Latitude;
                address.Longitude = dto.Longitude;

                pickupRequest = new PickupRequest
                {
                    CustomerId = customer.Id,
                    AddressId = address.Id,
                    ScheduledDate = dto.ScheduledDate,
                    CustomerNotes = dto.Notes,
                    Status = RequestStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    Details = dto.WasteCategoryIds.Select(categoryId => new RequestDetail
                    {
                        CategoryId = categoryId,
                        EstimatedWeight = 0,
                        ActualWeight = 0,
                        PointsEarned = 0
                    }).ToList()
                };
            }
            else
            {
                // 3b. No address exists — create a new one via navigation property
                pickupRequest = new PickupRequest
                {
                    CustomerId = customer.Id,
                    Address = new UserAddress
                    {
                        UserId = customer.Id,
                        Latitude = dto.Latitude,
                        Longitude = dto.Longitude,
                        City = "N/A"
                    },
                    ScheduledDate = dto.ScheduledDate,
                    CustomerNotes = dto.Notes,
                    Status = RequestStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    Details = dto.WasteCategoryIds.Select(categoryId => new RequestDetail
                    {
                        CategoryId = categoryId,
                        EstimatedWeight = 0,
                        ActualWeight = 0,
                        PointsEarned = 0
                    }).ToList()
                };
            }

            // 5. Save transactionally (EF Core tracks the entire graph)
            var created = await _requestRepository.AddPickupRequestAsync(pickupRequest);
            return created.Id;
        }

        // ─── Get Request History (Paged) ────────────────────────────────
        public async Task<PagedResult<RequestHistoryDto>> GetRequestHistoryAsync(
            int userId, int page, RequestStatus? status)
        {
            var customer = await _requestRepository.GetCustomerByUserIdAsync(userId);
            if (customer == null)
                throw new InvalidOperationException("Customer not found.");

            var totalCount = await _requestRepository.GetRequestCountByCustomerIdAsync(customer.Id, status);
            var requests = await _requestRepository.GetRequestsByCustomerIdAsync(customer.Id, page, DefaultPageSize, status);

            var items = requests.Select(r => new RequestHistoryDto
            {
                Id = r.Id,
                Status = r.Status.ToString(),
                ScheduledDate = r.ScheduledDate,
                CreatedAt = r.CreatedAt,
                AddressLabel = r.Address?.Label
            }).ToList();

            return new PagedResult<RequestHistoryDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = DefaultPageSize
            };
        }

        // ─── Get Request Details ────────────────────────────────────────
        public async Task<RequestDetailsResponseDto?> GetRequestDetailsAsync(int userId, int requestId)
        {
            var request = await _requestRepository.GetRequestByIdAsync(requestId);
            if (request == null)
                return null;

            // Ensure ownership
            if (request.CustomerId != userId)
                return null;

            return new RequestDetailsResponseDto
            {
                Id = request.Id,
                Status = request.Status.ToString(),
                ScheduledDate = request.ScheduledDate,
                CreatedAt = request.CreatedAt,
                CompletedDate = request.CompletedDate,
                CustomerNotes = request.CustomerNotes,
                TotalPointsEarned = request.TotalPointsEarned,
                Address = new RequestAddressDto
                {
                    Id = request.Address.Id,
                    Label = request.Address.Label,
                    Governorate = request.Address.Governorate,
                    City = request.Address.City,
                    StreetName = request.Address.StreetName
                },
                Items = request.Details.Select(d => new RequestDetailItemDto
                {
                    Id = d.Id,
                    CategoryName = d.Category.NameAr,
                    CategoryIcon = d.Category.IconUrl,
                    EstimatedWeight = d.EstimatedWeight,
                    ActualWeight = d.ActualWeight,
                    PointsEarned = d.PointsEarned
                }).ToList()
            };
        }

        // ─── Cancel Request ─────────────────────────────────────────────
        public async Task<bool> CancelRequestAsync(int userId, int requestId)
        {
            var request = await _requestRepository.GetRequestByIdAsync(requestId);
            if (request == null)
                return false;

            // Ensure ownership
            if (request.CustomerId != userId)
                throw new UnauthorizedAccessException("This request does not belong to you.");

            // Can only cancel if Pending
            if (request.Status != RequestStatus.Pending)
                throw new InvalidOperationException(
                    $"Cannot cancel a request with status '{request.Status}'. Only 'Pending' requests can be cancelled.");

            request.Status = RequestStatus.Cancelled;
            await _requestRepository.UpdateRequestAsync(request);
            return true;
        }
    }
}
