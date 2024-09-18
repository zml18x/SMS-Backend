using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Infrastructure.Identity.Entities;

namespace SpaManagementSystem.Infrastructure.Data.Context;

public class SmsDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Salon> Salons { get; set; }
    public DbSet<Employee> Employees { get; set; }
    
    
    
    public SmsDbContext() { }
    public SmsDbContext(DbContextOptions<SmsDbContext> options) : base(options) { }
    
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("SMS");
        
        modelBuilder.Entity<Salon>(entity =>
        {
            entity.HasKey(s => s.Id);
            
            entity.OwnsMany(s => s.OpeningHours, s =>
            {
                s.Property(oh => oh.DayOfWeek).IsRequired();
                s.Property(oh => oh.OpeningTime).IsRequired();
                s.Property(oh => oh.ClosingTime).IsRequired();
                
                s.WithOwner().HasForeignKey("SalonId");
                s.HasKey("SalonId", "DayOfWeek");
            });

            entity.OwnsOne(s => s.Address);

            entity.HasMany(s => s.Employees)
                .WithOne(e => e.Salon)
                .HasForeignKey(e => e.SalonId);
            
            entity.Property(s => s.Name).IsRequired();
            entity.Property(s => s.PhoneNumber).IsRequired();
            entity.Property(s => s.Email).IsRequired();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Salon)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.SalonId);

            entity.OwnsOne(e => e.Profile, p =>
            {
                p.ToTable("EmployeeProfiles");
                
                p.Property(ep => ep.FirstName).IsRequired();
                p.Property(ep => ep.LastName).IsRequired();
                p.Property(ep => ep.Gender).IsRequired();
                p.Property(ep => ep.DateOfBirth).IsRequired();
                p.Property(ep => ep.PhoneNumber).IsRequired();
                p.Property(ep => ep.Email).IsRequired();

                p.WithOwner().HasForeignKey("EmployeeId");
            });
            
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Position).IsRequired();
            entity.Property(e => e.Code).IsRequired();
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
        foreach (var role in Enum.GetNames(typeof(RoleTypes)))
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