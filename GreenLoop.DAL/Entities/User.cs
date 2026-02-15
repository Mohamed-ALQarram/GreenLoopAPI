using GreenLoop.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLoop.DAL.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } // Username

        [Required]
        public string PasswordHash { get; set; }

        public UserRole Role { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();


        // Relationship for Coupons (Partners)
        public ICollection<Coupon> PublishedCoupons { get; set; } = new List<Coupon>();
    }
}
