using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Data;

/// <summary>
/// A static class containing an extension method for database migration.
/// </summary>
public static class DatabaseMigrator
{
    /// <summary>
    /// Extension method for <see cref="IServiceProvider"/> that performs database migration.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to obtain the database context.</param>
    /// <returns>The same <see cref="IServiceProvider"/> that was passed in.</returns>
    public static IServiceProvider MigrateDatabase(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<SmsDbContext>();
            context.Database.Migrate();
        }

        return serviceProvider;
    }
}