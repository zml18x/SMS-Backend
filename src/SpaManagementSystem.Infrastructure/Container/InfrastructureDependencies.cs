using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;
using SpaManagementSystem.Infrastructure.Exceptions;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Infrastructure.Repositories;
using SpaManagementSystem.Infrastructure.Services;
using SpaManagementSystem.Application.Interfaces;

namespace SpaManagementSystem.Infrastructure.Container;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .ConfigureDatabase(configuration)
            .ConfigureIdentity()
            .ConfigureServices()
            .ConfigureJwt(configuration);

        return services;
    }
    
    private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SmsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("SmsConnection")));

        return services;
    }
    
    private static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<SmsDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
    
    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ISalonRepository, SalonRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddSingleton<IEmailSender<User>, EmailSender>();
        services.AddSingleton<IEmailService, EmailService>();
            
        return services;
    }
    
    private static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"] ??
                    throw new MissingConfigurationException("JWT key configuration is missing or empty")))
            };
        });

        return services;
    }
}