using GreenLoop.DAL.Data;
using GreenLoop.DAL.Entities;
using GreenLoop.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GreenLoop.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly GreenLoopDbContext _context;

        public AuthRepository(GreenLoopDbContext context)
        {
            _context = context;
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return await _context.Users
                .AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<User?> GetCustomerByPhoneAsync(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<User> AddUserAsync(User user)
        {
          
            await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }


    }
}
