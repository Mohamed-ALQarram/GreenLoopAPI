using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class PointTransaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Required]
        [ForeignKey("Driver")]
        public int DriverId { get; set; }
        public Driver Driver { get; set; }

        public int PointsAdded { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string QrCodeValue { get; set; }
    }
}
