using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Infrastructure.Data.Context
{
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
        public DbSet<OpeningHours> OpeningHours { get; set; }
        public DbSet<SalonAddress> SalonAddresses { get; set; }
        
        
        
        /// <summary>
        /// Configures the model schema and relationships for the database context during model creation.
        /// This method is called by the framework when the model for a derived context is being created.
        /// It is responsible for mapping the entity types to the database tables, configuring keys, indices, relationships,
        /// and setting the default schema for the context.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("SMS");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
