using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Data
{
    /// <summary>
    /// Provides functionality to manage and apply database migrations for different contexts within the Spa Management System.
    /// This class ensures that the database schema is correctly initialized and kept up-to-date with the latest changes
    /// for both the main business data and identity data.
    /// </summary>
    public static class DatabaseMigrator
    {
        /// <summary>
        /// Executes database migrations to ensure the database schema for both the main and identity contexts is up-to-date.
        /// This method checks the current database schemas and applies all necessary migrations to bring them to the latest version.
        /// If a schema does not exist, it will be created from scratch.
        /// This ensures both operational and identity data remain synchronized with application requirements.
        /// </summary>
        /// <param name="serviceProvider">An instance of <see cref="IServiceProvider"/>
        /// used for retrieving service objects and performing dependency injection.</param>
        public static void MigrateDatabase(IServiceProvider serviceProvider)
        {
            // Migrate the main business logic database context
            using (var scope = serviceProvider.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var context = scopeProvider.GetRequiredService<SmsDbContext>();
                context.Database.Migrate();
            }
            
            // Migrate the identity database context, which manages user authentication and authorization data
            using (var scope = serviceProvider.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var context = scopeProvider.GetRequiredService<SmsIdentityDbContext>();
                context.Database.Migrate();
            }
        }
    }
}