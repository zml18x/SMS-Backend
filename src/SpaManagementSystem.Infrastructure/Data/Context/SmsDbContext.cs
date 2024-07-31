using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Infrastructure.Data.Context;

/// <summary>
/// Represents the primary database context for the Spa Management System.
/// This context is used to configure and manage the database schema and relationships
/// for all business-related entities within the system.
/// </summary>
public class SmsDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the SmsDbContext class without specific options.
    /// This constructor is typically used for enabling design-time services such as migrations.
    /// </summary>
    public SmsDbContext() { }
        
    /// <summary>
    /// Initializes a new instance of the SmsDbContext class with specific options.
    /// The provided options configure the database and other settings that will be used by this context.
    /// </summary>
    /// <param name="options">The options for configuring the context.</param>
    public SmsDbContext(DbContextOptions<SmsDbContext> options) : base(options) { }

        
        
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Salon> Salons { get; set; }
        
        
        
    /// <summary>
    /// Configures the model schema and relationships for the database context during model creation.
    /// This method is called by the framework when the model for a derived context is being created.
    /// It is responsible for mapping the entity types to the database tables, configuring keys, indices, relationships,
    /// and setting the default schema for the context.
    /// </summary>
    /// <param name="modelBuilder">The model builder instance used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.Property(x => x.Id).IsRequired();
            entity.Property(x => x.UserId).IsRequired();
            entity.Property(x => x.FirstName).IsRequired();
            entity.Property(x => x.LastName).IsRequired();
            entity.Property(x => x.Gender).IsRequired();
            entity.Property(x => x.DateOfBirth).IsRequired();
            
            entity.HasKey(x => x.Id);
        });
        
        modelBuilder.Entity<Salon>(entity =>
        {
            entity.Property(x => x.Id).IsRequired();
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.PhoneNumber).IsRequired();
            entity.Property(x => x.Email).IsRequired();
            
            entity.HasKey(x => x.Id);
            
            entity.OwnsMany(x => x.OpeningHours, s =>
            {
                s.Property(oh => oh.DayOfWeek).IsRequired();
                s.Property(oh => oh.OpeningTime).IsRequired();
                s.Property(oh => oh.ClosingTime).IsRequired();
                s.WithOwner().HasForeignKey("SalonId");
                s.HasKey("SalonId", "DayOfWeek");
            });

            entity.OwnsOne(x => x.Address);
        });
        
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("SMS");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}