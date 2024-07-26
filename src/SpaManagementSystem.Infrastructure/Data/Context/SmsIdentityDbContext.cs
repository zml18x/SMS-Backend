using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SpaManagementSystem.Infrastructure.Identity.Enums;
using SpaManagementSystem.Infrastructure.Identity.Entities;

namespace SpaManagementSystem.Infrastructure.Data.Context;

/// <summary>
/// Represents the database context for managing identity and authorization data within the Spa Management System.
/// This context handles the storage and retrieval of user and role information, extending the IdentityDbContext
/// to utilize custom user and role types with a GUID as the identifier.
/// </summary>
public class SmsIdentityDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    /// <summary>
    /// Initializes a new instance of the SmsIdentityDbContext.
    /// </summary>
    public SmsIdentityDbContext() { }
        
    /// <summary>
    /// Initializes a new instance of the SmsIdentityDbContext with specific options.
    /// The options configure the database (and other options) to be used for this context.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public SmsIdentityDbContext(DbContextOptions<SmsIdentityDbContext> options) : base(options) { }



    /// <summary>
    /// Configures the model schema, table names, relationships, and seed data for the Identity database context.
    /// This method is called by the framework when the model for a derived context has been initialized and 
    /// maps the entity types to database tables, configures keys and indices, and sets up any relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("SMS");

        // Customize table names for clarity and to follow specific naming conventions within the database.
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        // Seed initial roles into the database from the RoleType enum.
        foreach (var role in Enum.GetNames(typeof(RoleType)))
        {
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles").HasData(new IdentityRole<Guid>
            {
                Name = role,
                NormalizedName = role.ToUpper(),
                Id = Guid.NewGuid(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });
        }

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}