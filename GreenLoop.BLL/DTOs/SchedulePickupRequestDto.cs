using System.ComponentModel.DataAnnotations;

namespace GreenLoop.BLL.DTOs
{
    public class SchedulePickupRequestDto
    {
        [Required]
        public int AddressId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one waste category is required.")]
        public List<int> WasteCategoryIds { get; set; } = new();

        [Required]
        public DateTime ScheduledDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
