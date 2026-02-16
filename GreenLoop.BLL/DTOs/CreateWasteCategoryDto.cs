using System.ComponentModel.DataAnnotations;

namespace GreenLoop.BLL.DTOs
{
    public class CreateWasteCategoryDto
    {
        [Required]
        [MaxLength(50)]
        public string NameAr { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int PointsPerKg { get; set; }

        [Range(0, (double)decimal.MaxValue)]
        public decimal MarketPricePerTon { get; set; }

        public string? IconUrl { get; set; }
    }
}
