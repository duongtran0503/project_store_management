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
            services.AddScoped<AuthorRepository>();
            services.AddScoped<PublisherRepository>();
            //Add Service
            services.AddScoped<ProductService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<AuthorService>();
            services.AddScoped<PublisherService>();
            return services;
        }
    }
}
