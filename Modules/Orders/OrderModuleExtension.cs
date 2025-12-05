using StoreManagement.API.Modules.Orders.Repository;
using StoreManagement.API.Modules.Orders.Services;

namespace StoreManagement.API.Modules.Orders
{
    public static class OrderModuleExtension
    {
        public static IServiceCollection AddOrdersModule(this IServiceCollection services) {
            services.AddScoped<InvoiceService>();
            services.AddScoped<InvoiceRepository>();

            return services;
        }
    }
}
