using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{

    public class Coupon
    {
        [Key]
        public Guid Id { get; set; }

        // Keeping BusinessId as int if Business.Id is int, assuming Business entity exists and has int Id
        [ForeignKey("Business")]
        public int? BusinessId { get; set; } // Made nullable as it might not be strictly required for global coupons
        public Business? Business { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public int RequiredPoints { get; set; } // Renamed from PointsCost as per request, or mapping it. Request said 'RequiredPoints'

        public DateTime? ExpiryDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

