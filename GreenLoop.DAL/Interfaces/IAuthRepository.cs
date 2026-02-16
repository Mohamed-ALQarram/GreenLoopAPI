using GreenLoop.DAL.Entities;

namespace GreenLoop.DAL.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> PhoneNumberExistsAsync(string phoneNumber);
        Task<Customer?> GetCustomerByPhoneAsync(string phoneNumber);
        Task<User> AddUserAsync(User user);
    }
}
