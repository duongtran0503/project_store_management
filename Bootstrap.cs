using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using StoreManagement.API.Common.Middleware;
using StoreManagement.API.Modules.Authentication;
using StoreManagement.API.Modules.Authentication.Services;
using StoreManagement.API.Modules.Inventories;
using StoreManagement.API.Modules.Orders;
using StoreManagement.API.Modules.Products;
using StoreManagement.API.Modules.Promotions;
using StoreManagement.API.Modules.Report;
using StoreManagement.API.Modules.Suppliers;
using StoreManagement.API.Modules.Users;
using StoreManagement.API.Shared.Data;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoreManagement.API;

public static class Bootstrap
{
    public static readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
       
            builder.AddMvcServices()
               .AddSwaggerServices()
               .AddDatabaseServices()
               .AddAuthenticationServices()
               .AddAuthorizationServices()
               .AddHttpContextServices()
               .AddApplicationModules()
               .AddCors();
                

        return builder;
    }

    // Add Module
    public static WebApplicationBuilder AddApplicationModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthenticationModule();
        builder.Services.AddUserModule();
        builder.Services.AddProductModule();
        builder.Services.AddInventoryModule();
        builder.Services.AddReportModule();
        builder.Services.AddOrdersModule();
        builder.Services.AddPromotionModule();
        builder.Services.AddSupplierModule();
        return builder;
    }
    public static WebApplicationBuilder AddMvcServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
              
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
                opt.JsonSerializerOptions.WriteIndented = true;
                opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            })
            ;

        return builder;
    }

    public static WebApplicationBuilder AddSwaggerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    public static WebApplicationBuilder AddDatabaseServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 21)),
                mySqlOptions =>
                {
                   
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null)
                    .CommandTimeout(30);
                }
            ));

        return builder;
    }

    public static WebApplicationBuilder AddAuthenticationServices(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"];

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                ClockSkew = TimeSpan.Zero,
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        });

        return builder;
    }


    public static WebApplicationBuilder AddAuthorizationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization();

        return builder;
    }

    public static WebApplicationBuilder AddHttpContextServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor(); 


        return builder;
    }

    public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {

                                  policy.WithOrigins("http://localhost:3000")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                              });
        });
        return builder;
    }

    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<LoggingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors(MyAllowSpecificOrigins);

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    // Auto Initial Admin Account
    public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAdminUserAsync();

        return app;
    }

    public class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
           
            return reader.GetDateTime().ToUniversalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            
            writer.WriteStringValue(value.ToUniversalTime().ToString("o"));
        }
    }
}
