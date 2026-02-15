using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class Customer: User
    {
        [InverseProperty("Customer")]
        public ICollection<PickupRequest> Requests { get; set; } = new List<PickupRequest>();
        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();

        //[InverseProperty("Customer")]
        //public ICollection<CouponRedemption> Redemptions { get; set; } = new List<CouponRedemption>();

    }
}
