using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class RequestDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Request")]
        public int RequestId { get; set; }
        public PickupRequest Request { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public WasteCategory Category { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal EstimatedWeight { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal ActualWeight { get; set; }

        public int PointsEarned { get; set; }
    }
}
