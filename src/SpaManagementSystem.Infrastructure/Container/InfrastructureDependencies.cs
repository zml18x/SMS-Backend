using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Infrastructure.Data.Context;
using SpaManagementSystem.Infrastructure.Identity.Entities;

namespace SpaManagementSystem.Infrastructure.Container
{
    /// <summary>
    /// Provides a centralized class for configuring essential infrastructure layer services required by the application.
    /// This class includes configurations for dependency injection, database contexts, and identity services,
    /// facilitating the management of data access and user authentication.
    /// </summary>
    public static class InfrastructureDependencies
    {
        /// <summary>
        /// Configures core services and dependencies for the entire application.
        /// This extension method serves as the central setup function for registering various services such as database contexts,
        /// identity management, and authentication mechanisms.
        /// It modifies and returns the IServiceCollection to allow for fluent configuration.
        /// </summary>
        /// <param name="services">The collection of service descriptors for registering application services.</param>
        /// <param name="configuration">The application-wide settings and configurations,
        /// used for setting up specific services like database connections and identity policies.</param>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .ConfigureDatabase(configuration)
                .ConfigureIdentity();

            return services;
        }
        
        /// <summary>
        /// Sets up the database contexts for the application using PostgreSQL,
        /// ensuring that all database interactions are efficiently configured.
        /// This method configures both the main and identity contexts with
        /// connections specified in the application settings, optimizing data management and access patterns.
        /// </summary>
        /// <param name="services">The collection of services where database contexts are added.</param>
        /// <param name="configuration">The application settings containing database connection strings.</param>
        private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SmsDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("SmsConnection")));
            
            services.AddDbContext<SmsIdentityDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("SmsConnection")));

            return services;
        }
        
        /// <summary>
        /// Configures the identity framework services for managing user authentication and authorization within the application.
        /// This method sets up the Microsoft.AspNetCore.Identity services with stringent policy requirements
        /// for user management, such as password complexity and email verification, enhancing security and user management efficacy.
        /// </summary>
        /// <param name="services">The collection of services where identity services are configured.</param>
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

                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<SmsIdentityDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
