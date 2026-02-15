using System.ComponentModel.DataAnnotations;

namespace GreenLoop.DAL.Entities
{
    public class WasteCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string NameAr { get; set; }

        public int PointsPerKg { get; set; } // Buy Price (Points)

        public decimal MarketPricePerTon { get; set; } // Sell Price (Money)

        public string? IconUrl { get; set; }
    }
}
