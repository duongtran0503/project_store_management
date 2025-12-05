using StoreManagement.API.Modules.Users.Repository;
using StoreManagement.API.Modules.Users.Services;

namespace StoreManagement.API.Modules.Users
{
    public static class UserModuleExtension
    {
        public static IServiceCollection AddUserModule(this IServiceCollection services) {
            // add service
            services.AddScoped<UserService>();
            services.AddScoped<CustomerService>();

            // Add repository
            services.AddScoped<UserRepository>();
            services.AddScoped<CustomerRepository>();

            return services;
        }
    }
}
