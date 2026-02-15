using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class Partner: Customer
    {
        [Required]
        [MaxLength(100)]
        public string BusinessName { get; set; }

        [MaxLength(50)]
        public string? CommercialRegister { get; set; } 

        [InverseProperty("Partner")]
        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
    }
}
