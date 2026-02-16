using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.BLL.DTOs
{
    public class SchedulePickupRequestDto
    {

        [Required]
        [MinLength(1, ErrorMessage = "At least one waste category is required.")]
        public List<int> WasteCategoryIds { get; set; } = new();

        [Required]

        [Column(TypeName = "decimal(18, 9)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(18, 9)")]
        public decimal Longitude { get; set; }

        public DateTime ScheduledDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}
