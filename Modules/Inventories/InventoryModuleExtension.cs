using StoreManagement.API.Modules.Inventories.Repository;
using StoreManagement.API.Modules.Inventories.Services;

namespace StoreManagement.API.Modules.Inventories

{
    public static  class InventoryModuleExtension
    {
        public static IServiceCollection AddInventoryModule(this IServiceCollection services) {

            services.AddScoped<InventoryRepository>();
            services.AddScoped<InventoryService>();

            return services;
        }
    }
}
