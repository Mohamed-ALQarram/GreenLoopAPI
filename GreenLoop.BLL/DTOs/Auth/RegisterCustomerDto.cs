using System.ComponentModel.DataAnnotations;

namespace GreenLoop.BLL.DTOs.Auth
{
    public class RegisterCustomerDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? AddressCity { get; set; }
    }
}
