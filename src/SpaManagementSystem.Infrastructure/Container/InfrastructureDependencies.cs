using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Infrastructure.Data.Context;

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
        }
    }
}