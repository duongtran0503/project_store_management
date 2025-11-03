using StoreManagement.API.Modules.Products.Repository;
using StoreManagement.API.Modules.Products.Services;

namespace StoreManagement.API.Modules.Products
{
    public static class ProductModuleExtension
    {
        public static IServiceCollection AddProductModule(this IServiceCollection services) {

            // Add repository
            services.AddScoped<ProductRepository>();
            services.AddScoped<CategoryRepository>();
            //Add Service
            services.AddScoped<ProductService>();
            services.AddScoped<CategoryService>();
            return services;
        }
    }
}
