using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Infrastructure.Data.Context;
using SpaManagementSystem.Infrastructure.Identity.Entities;

namespace SpaManagementSystem.Infrastructure.Container
{
    /// <summary>
    /// Provides a centralized class for configuring essential infrastructure services required by the application.
    /// This class includes configurations for dependency injection, database context, identity services, and JWT authentication.
    /// </summary>
    public static class InfrastructureDependencies
    {
        /// <summary>
        /// Configures core services and dependencies for the entire application.
        /// This method serves as the central setup function for registering various services like database contexts,
        /// identity management, and authentication mechanisms.
        /// </summary>
        /// <param name="services">The collection of service descriptors for registering application services.</param>
        /// <param name="configuration">The application-wide settings and configurations,
        /// used for setting up specific services like database connections and JWT settings.</param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureDatabase(services, configuration);
            ConfigureIdentity(services);
        }
        
        /// <summary>
        /// Sets up the database contexts for the application using PostgreSQL,
        /// ensuring that all database interactions are configured.
        /// </summary>
        /// <param name="services">The collection of services where database contexts are added.</param>
        /// <param name="configuration">The application settings containing database connection strings.</param>
        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SmsDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("SmsConnection"));
            });
            
            services.AddDbContext<SmsIdentityDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("SmsConnection"));
            });
        }
        
        /// <summary>
        /// Configures the identity framework services for managing user authentication and authorization within the application.
        /// </summary>
        /// <param name="services">The collection of services where identity services are configured.</param>
        private static void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequiredLength = 10;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireDigit = true;

                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<SmsIdentityDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}