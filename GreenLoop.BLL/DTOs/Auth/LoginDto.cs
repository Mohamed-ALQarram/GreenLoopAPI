using System.ComponentModel.DataAnnotations;

namespace GreenLoop.BLL.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
