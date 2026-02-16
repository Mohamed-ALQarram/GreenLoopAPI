using GreenLoop.BLL.DTOs.Auth;

namespace GreenLoop.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterCustomerAsync(RegisterCustomerDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
