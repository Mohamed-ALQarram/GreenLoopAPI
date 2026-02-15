using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Partner")]
        public int PartnerId { get; set; }
        public User Partner { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } // Unique

        public int PointsCost { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
