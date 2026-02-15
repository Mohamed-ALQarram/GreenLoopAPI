using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenLoop.DAL.Entities
{
    public class Business: User
    {
        [Required]
        [MaxLength(100)]
        public string BusinessName { get; set; }

        [MaxLength(50)]
        public string? CommercialRegister { get; set; }
        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();

    }
}
