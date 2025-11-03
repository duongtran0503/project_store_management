
using StoreManagement.API.Modules.Authentication.Mapper;
using StoreManagement.API.Modules.Authentication.Repository;
using StoreManagement.API.Modules.Authentication.Services;

namespace StoreManagement.API.Modules.Authentication
{
    public static class AuthenticationModuleExtentions
    {
        public static IServiceCollection AddAuthenticationModule(this IServiceCollection services) {
            // Add services
            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();
            services.AddScoped<AuthTokenService>();
            // Add Repository
            services.AddScoped<UserRepository>();
          

            // Add mapper
            services.AddScoped<UserMapper>();

            // Add auto create account admin
            services.AddScoped<DataSeeder>();
            return services;
        }
    }
}
