using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class Driver: User
    {
        [MaxLength(50)]
        public string VehicleType { get; set; } // Truck, Tricycle

        [MaxLength(50)]
        public string LicenseNumber { get; set; }

        public bool IsVerified { get; set; } = false;

        public int? CurrentZoneId { get; set; } // FK to Zone

        // علاقات السائق فقط
        [InverseProperty("Driver")]
        public ICollection<PickupRequest> Tasks { get; set; } = new List<PickupRequest>();

        public ICollection<DriverDocument> Documents { get; set; } = new List<DriverDocument>();
    }
}
