using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Infrastructure.Identity.Enums;

namespace SpaManagementSystem.Infrastructure.Data.Context;

public class SmsDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public SmsDbContext() { }
    public SmsDbContext(DbContextOptions<SmsDbContext> options) : base(options) { }
    
    
    
    public DbSet<Salon> Salons { get; set; }
    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("SMS");
        
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