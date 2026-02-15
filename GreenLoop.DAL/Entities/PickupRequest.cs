using GreenLoop.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class PickupRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public UserAddress Address { get; set; }

        [ForeignKey("Driver")]
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? CustomerNotes { get; set; }

        public string? DriverProofImage { get; set; }

        public int TotalPointsEarned { get; set; } = 0;

        public ICollection<RequestDetail> Details { get; set; } = new List<RequestDetail>();
    }
}
