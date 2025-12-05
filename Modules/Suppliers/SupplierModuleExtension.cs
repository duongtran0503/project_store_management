using StoreManagement.API.Modules.Suppliers.Repository;
using StoreManagement.API.Modules.Suppliers.Services;

namespace StoreManagement.API.Modules.Suppliers
{
    public static class SupplierModuleExtension
    {
        public static IServiceCollection AddSupplierModule(this IServiceCollection services) {
            // Add Repository
            services.AddScoped<SupplierRepository>();
            // Add service
            services.AddScoped<SupplierService>();
            return services;
        }
    }
}
