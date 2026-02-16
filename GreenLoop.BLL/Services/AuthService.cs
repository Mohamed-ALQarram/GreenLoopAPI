using GreenLoop.BLL.DTOs.Auth;
using GreenLoop.BLL.Interfaces;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Enums;
using GreenLoop.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GreenLoop.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly SymmetricSecurityKey _key;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;

            var tokenKey = configuration["TokenKey"]
                ?? throw new InvalidOperationException("TokenKey is not configured in appsettings.");

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }

        // ─── Register ────────────────────────────────────────────────────
        public async Task<AuthResponseDto> RegisterCustomerAsync(RegisterCustomerDto dto)
        {
            // 1. Check if phone number already exists
            var exists = await _authRepository.PhoneNumberExistsAsync(dto.PhoneNumber);

            if (exists)
                throw new InvalidOperationException("User already exists.");

            // 2. Create Customer entity
            var customer = new Customer
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.Customer
            };

            // 3. Save to DB via repository
            await _authRepository.AddCustomerAsync(customer);

            // 4. Generate JWT and return response
            return BuildAuthResponse(customer);
        }

        // ─── Login ───────────────────────────────────────────────────────
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // 1. Find customer by phone number
            var customer = await _authRepository.GetCustomerByPhoneAsync(dto.PhoneNumber);

            if (customer is null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            // 2. Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials.");

            // 3. Generate JWT and return response
            return BuildAuthResponse(customer);
        }

        // ─── Helpers ─────────────────────────────────────────────────────

        private AuthResponseDto BuildAuthResponse(Customer customer)
        {
            var expiresAt = DateTime.UtcNow.AddDays(7);
            var token = GenerateToken(customer, expiresAt);

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserName = customer.FullName,
                Role = nameof(UserRole.Customer)
            };
        }

        private string GenerateToken(Customer customer, DateTime expiresAt)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                new(ClaimTypes.Name, customer.FullName),
                new(ClaimTypes.Role, nameof(UserRole.Customer))
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
