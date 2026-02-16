using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class UserCoupon
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Coupon")]
        public Guid CouponId { get; set; }
        public Coupon Coupon { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } // Unique code generated upon redemption

        public DateTime RedeemedDate { get; set; } = DateTime.UtcNow;

        public bool IsUsed { get; set; } = false; // If the coupon has been used at the vendor
    }
}
