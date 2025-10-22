
using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Modules.Authentication;
using StoreManagement.API.Shared.Data;
using StoreManagement.API.Shared.Middleware;

namespace StoreManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
           

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Connect database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21)),
        mySqlOptions =>
        {
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }
    ));

           
            // Register all modules

            builder.Services.AddAuthenticationModule();
          
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }
         
            app.UseMiddleware<LoggingMiddleware>();

            app.UseAuthorization();

          

            app.MapControllers();

            app.Run();
        }
    }
}
