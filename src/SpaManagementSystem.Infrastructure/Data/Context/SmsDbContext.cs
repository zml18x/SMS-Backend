using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace SpaManagementSystem.Infrastructure.Data.Context
{
    /// <summary>
    /// Represents the database context for the SpaManagementSystem.
    /// </summary>
    public class SmsDbContext : DbContext
    {
        public SmsDbContext() { }
        public SmsDbContext(DbContextOptions<SmsDbContext> options) : base(options) { }

        
        
        /// <summary>
        /// Configures the model schema and relationships for the database context.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("SMS");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
