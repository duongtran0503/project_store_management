using StoreManagement.API.Modules.Authentication.Services;

namespace StoreManagement.API.Modules.Authentication
{
    public static class AuthenticationModuleExtentions
    {
        public static IServiceCollection AddAuthenticationModule(this IServiceCollection services) {
            // Add services
            services.AddScoped<TestService>();
            // Add Repository
            return services;
        }
    }
}
