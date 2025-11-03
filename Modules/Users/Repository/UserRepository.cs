using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Shared.Data;

namespace StoreManagement.API.Modules.Users.Repository
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) { 
           _context = context;
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
