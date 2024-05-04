using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Data
{
    /// <summary>
    /// Provides functionality to manage and apply database migrations for different contexts within the Spa Management System.
    /// This class ensures that the database schemas are correctly initialized and kept up-to-date with the latest changes
    /// for both the main business data and identity data, facilitating consistent and synchronized database management across the system.
    /// </summary>
    public static class DatabaseMigrator
    {
        /// <summary>
        /// Executes database migrations to ensure the database schema for both the main and identity contexts is up-to-date.
        /// This extension method checks the current database schemas and applies all necessary migrations to bring them to the latest version.
        /// If a schema does not exist, it will be created from scratch. This ensures that both operational and identity data remain synchronized
        /// with application requirements, maintaining system integrity and functionality.
        /// </summary>
        /// <param name="serviceProvider">An instance of <see cref="IServiceProvider"/>
        /// used for retrieving service objects and performing dependency injection. This provider facilitates access to database contexts
        /// for migration purposes.</param>
        /// <returns>The <see cref="IServiceProvider"/> instance to support chaining and further configuration.</returns>
        public static IServiceProvider MigrateDatabase(this IServiceProvider serviceProvider)
        {
            // Migrate the main business logic database context
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SmsDbContext>();
                context.Database.Migrate();
            }
            
            // Migrate the identity database context, which manages user authentication and authorization data
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SmsIdentityDbContext>();
                context.Database.Migrate();
            }

            return serviceProvider;
        }
    }
}