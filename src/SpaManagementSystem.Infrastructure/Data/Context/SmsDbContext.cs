using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Infrastructure.Identity.Entities;

namespace SpaManagementSystem.Infrastructure.Data.Context;

public class SmsDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Salon> Salons { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<EmployeeAvailability> EmployeeAvailabilities { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppointmentService> AppointmentServices { get; set; }
    
    
    
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

            entity.HasMany(s => s.Products)
                .WithOne(p => p.Salon)
                .HasForeignKey(p => p.SalonId);
            
            entity.HasMany(s => s.Services)
                .WithOne(s => s.Salon)
                .HasForeignKey(s => s.SalonId);
            
            entity.HasMany(s => s.Customers)
                .WithOne(c => c.Salon)
                .HasForeignKey(c => c.SalonId);
            
            entity.HasMany(s => s.Appointments)
                .WithOne(a => a.Salon)
                .HasForeignKey(a => a.SalonId);
            
            entity.Property(s => s.Name).IsRequired();
            entity.Property(s => s.PhoneNumber).IsRequired();
            entity.Property(s => s.Email).IsRequired();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne<User>()
                .WithOne()
                .HasForeignKey<Employee>(e => e.UserId)
                .IsRequired();
            
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
            
            entity.HasMany(e => e.Services)
                .WithMany(s => s.Employees)
                .UsingEntity(j => j.ToTable("EmployeeServices"));

            entity.HasMany(e => e.EmployeeAvailabilities)
                .WithOne(ea => ea.Employee)
                .HasForeignKey(ea => ea.EmployeeId);
            
            entity.HasMany(e => e.Appointments)
                .WithOne(a => a.Employee)
                .HasForeignKey(e => e.EmployeeId);
            
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Position).IsRequired();
            entity.Property(e => e.Code).IsRequired();
        });

        modelBuilder.Entity<EmployeeAvailability>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity
                .HasOne(ea => ea.Employee)
                .WithMany(e => e.EmployeeAvailabilities)
                .HasForeignKey(ea => ea.EmployeeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(ea => ea.Employee)
                .WithMany(e => e.EmployeeAvailabilities)
                .HasForeignKey(ea => ea.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.OwnsMany(e => e.AvailableHours, p =>
            {
                p.ToTable("EmployeeAvailabilityHours");

                p.Property(h => h.Start).IsRequired();
                p.Property(h => h.End).IsRequired();

                p.WithOwner().HasForeignKey("EmployeeAvailabilityId");
            });
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasOne(p => p.Salon)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SalonId);

            entity.Property(p => p.SalonId).IsRequired();
            entity.Property(p => p.Name).IsRequired();
            entity.Property(p => p.Code).IsRequired();
            entity.Property(p => p.PurchasePrice).IsRequired();
            entity.Property(p => p.PurchaseTaxRate).IsRequired();
            entity.Property(p => p.SalePrice).IsRequired();
            entity.Property(p => p.SaleTaxRate).IsRequired();
            entity.Property(p => p.StockQuantity).IsRequired();
            entity.Property(p => p.MinimumStockQuantity).IsRequired();
            entity.Property(p => p.IsActive).IsRequired();
        });
        
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.HasOne(s => s.Salon)
                .WithMany(s => s.Services)
                .HasForeignKey(s => s.SalonId);
            
            entity.Property(s => s.SalonId).IsRequired();
            entity.Property(s => s.Name).IsRequired();
            entity.Property(s => s.Code).IsRequired();
            entity.Property(s => s.Price).IsRequired();
            entity.Property(s => s.TaxRate).IsRequired();
            entity.Property(s => s.Duration).IsRequired();
            entity.Property(s => s.IsActive).IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            
            entity.HasOne(c => c.Salon)
                .WithMany(s => s.Customers)
                .HasForeignKey(c => c.SalonId);
            
            entity.HasMany(c => c.Appointments)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId);
            
            entity.Property(c => c.SalonId).IsRequired();
            entity.Property(c => c.FirstName).IsRequired();
            entity.Property(c => c.LastName).IsRequired();
            entity.Property(c => c.Gender).IsRequired();
            entity.Property(c => c.PhoneNumber).IsRequired();
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.Id);
            
            entity.HasOne(p => p.Salon)
                .WithMany()
                .HasForeignKey(p => p.SalonId);
            
            entity.HasOne(p => p.Appointment)
                .WithMany(a => a.Payments)
                .HasForeignKey(p => p.AppointmentId);
            
            entity.Property(p => p.SalonId).IsRequired();
            entity.Property(p => p.AppointmentId).IsRequired();
            entity.Property(p => p.Amount).IsRequired();
            entity.Property(p => p.PaymentDate).IsRequired();
            entity.Property(p => p.Status).IsRequired();
            entity.Property(p => p.Method).IsRequired();
        });

        modelBuilder.Entity<AppointmentService>(entity =>
        {
            entity.HasKey(s => s.Id);
            
            entity.HasOne(s => s.Appointment)
                .WithMany(a => a.AppointmentServices)
                .HasForeignKey(s => s.AppointmentId);
            
            entity.HasOne(s => s.Service)
                .WithMany()
                .HasForeignKey(s => s.ServiceId);
            
            entity.Property(s => s.SalonId).IsRequired();
            entity.Property(s => s.AppointmentId).IsRequired();
            entity.Property(s => s.ServiceId).IsRequired();
            entity.Property(s => s.Price).IsRequired();
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);
            
            entity.HasOne(a => a.Salon)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.SalonId);
            
            entity.HasOne(a => a.Employee)
                .WithMany(e => e.Appointments)
                .HasForeignKey(a => a.EmployeeId);

            entity.HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId);
            
            entity.HasMany(a => a.AppointmentServices)
                .WithOne(a => a.Appointment)
                .HasForeignKey(a => a.AppointmentId);

            entity.HasMany(a => a.Payments)
                .WithOne(p => p.Appointment)
                .HasForeignKey(p => p.AppointmentId);
            
            entity.Property(a => a.SalonId).IsRequired();
            entity.Property(a => a.EmployeeId).IsRequired();
            entity.Property(a => a.CustomerId).IsRequired();
            entity.Property(a => a.Date).IsRequired();
            entity.Property(a => a.StartTime).IsRequired();
            entity.Property(a => a.EndTime).IsRequired();
            entity.Property(a => a.Status).IsRequired();
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