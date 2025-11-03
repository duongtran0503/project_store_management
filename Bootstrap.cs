using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreManagement.API.Common.Middleware;
using StoreManagement.API.Modules.Authentication;
using StoreManagement.API.Modules.Authentication.Services;
using StoreManagement.API.Modules.Products;
using StoreManagement.API.Modules.Users;
using StoreManagement.API.Shared.Data;
using System.Text;

namespace StoreManagement.API;

public static class Bootstrap
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {

        builder.AddMvcServices()
               .AddSwaggerServices()
               .AddDatabaseServices()
               .AddAuthenticationServices()
               .AddAuthorizationServices()
               .AddHttpContextServices()
               .AddApplicationModules();

        return builder;
    }

    // Add Module
    public static WebApplicationBuilder AddApplicationModules(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthenticationModule();
        builder.Services.AddUserModule();
        builder.Services.AddProductModule();

        return builder;
    }
    public static WebApplicationBuilder AddMvcServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

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
                        errorNumbersToAdd: null);
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

    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<LoggingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

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
}
