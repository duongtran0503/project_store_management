using StoreManagement.API.Modules.Promotions.Repository;
using StoreManagement.API.Modules.Promotions.Services;

namespace StoreManagement.API.Modules.Promotions
{
    public static class PromotionModuleExtension
    {
        public static IServiceCollection AddPromotionModule(this IServiceCollection services) {

            services.AddScoped<VoucherRepository>();

            services.AddScoped<VoucherService>();

            return services;
        }
    }
}
