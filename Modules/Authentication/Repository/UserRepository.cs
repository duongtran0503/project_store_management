using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
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

         public async Task<Account> RegisterAsync(Account us)
        {
           await _context.AddAsync(us);
           await _context.SaveChangesAsync();
            return us;
        }

        public async Task<bool> CheckUserByEmailAsync(string email)
        {
            return await _context.Accounts.AnyAsync(u => u.Email == email);
        }

        public async Task<Account> GetUserByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Account> CreateUserAsync(Account user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Account> GetUserById(string id)
        {
            return await _context.Accounts
                .Where(u => u.Id == id)
                .Select(u => new Account
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    RoleName = u.RoleName,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

    }
}
