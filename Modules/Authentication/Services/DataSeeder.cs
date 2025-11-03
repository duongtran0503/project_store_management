using StoreManagement.API.Modules.Authentication.Constants;
using StoreManagement.API.Modules.Authentication.Repository;
using StoreManagement.API.Shared.Entities;

namespace StoreManagement.API.Modules.Authentication.Services
{
    public class DataSeeder
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(UserRepository userRepository, ILogger<DataSeeder> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task SeedAdminUserAsync()
        {
            try
            {
    
                var existingAdmin = await _userRepository.GetUserByEmailAsync("admin@gmail.com");

                if (existingAdmin != null)
                {
                    _logger.LogInformation("✅ Admin user already exists");
                    return;
                }

          
                var adminUser = new Common.Entities.Account
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = "admin",
                    Email = "admin@gmail.com",
                    PositionName = "Quản lý cửa hàng",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123123"),
                    RoleName = Roles.ADMIN.ToString(),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.CreateUserAsync(adminUser);

                _logger.LogInformation("✅ Admin user created successfully: {Email}", adminUser.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error seeding admin user");
                throw;
            }
        }
    }
}
