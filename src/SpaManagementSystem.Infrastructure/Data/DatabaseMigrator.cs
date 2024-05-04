using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Data
{
    /// <summary>
    /// Provides functionality to manage and apply database migrations.
    /// This class ensures that the database schema is correctly initialized and kept up-to-date with the latest changes.
    /// </summary>
    public static class DatabaseMigrator
    {
        /// <summary>
        /// Executes database migrations to ensure the database schema is up-to-date.
        /// This method checks the current database schema and applies all necessary migrations to bring it to the latest version.
        /// If the schema does not exist, it will be created from scratch.
        /// </summary>
        /// <param name="serviceProvider">An instance of <see cref="IServiceProvider"/>
        /// used for retrieving service objects and performing dependency injection.</param>
        public static void MigrateDatabase(IServiceProvider serviceProvider)
        {
            // Migrate the main database context
            using (var scope = serviceProvider.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;
                var context = scopeProvider.GetRequiredService<SmsDbContext>();
                context.Database.Migrate();
            }
        }
    }
}