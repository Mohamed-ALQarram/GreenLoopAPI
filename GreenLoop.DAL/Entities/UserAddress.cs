using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class UserAddress
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [MaxLength(50)]
        public string? Label { get; set; } // Home, Work

        [MaxLength(50)]
        public string? Governorate { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(100)]
        public string? StreetName { get; set; }

        [Column(TypeName = "decimal(18, 9)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(18, 9)")]
        public decimal Longitude { get; set; }

        public bool IsDefault { get; set; } = false;
    }
}
