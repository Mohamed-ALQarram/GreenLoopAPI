using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class Customer: User
    {
        [InverseProperty("Customer")]
        public ICollection<PickupRequest> Requests { get; set; } = new List<PickupRequest>();
        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
        
        public int PointsBalance { get; set; } = 0;
        public int TotalPointsEarned { get; set; } = 0;

        [InverseProperty("Customer")]
        public ICollection<UserCoupon> Redemptions { get; set; } = new List<UserCoupon>();

    }
}
