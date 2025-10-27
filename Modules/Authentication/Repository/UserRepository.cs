using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Shared.Data;
using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Modules.Authentication.Repository
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) { 
         _context = context;
        }

         public async Task<User> RegisterAsync(User us)
        {
           await _context.AddAsync(us);
           await _context.SaveChangesAsync();
            return us;
        }

        public async Task<bool> CheckUserByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new User
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

    }
}
