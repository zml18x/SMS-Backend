using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class CustomerRepository(SmsDbContext context) : Repository<Customer>(context), ICustomerRepository
{
    private readonly SmsDbContext _context = context;
    
    public async Task<Customer?> GetByPhoneNumberAsync(Guid salonId, string phoneNumber)
        => await _context.Customers.FirstOrDefaultAsync(x => x.SalonId == salonId && x.PhoneNumber == phoneNumber);

    public async Task<Customer?> GetByEmailAsync(Guid salonId, string email)
        => await _context.Customers
            .FirstOrDefaultAsync(x => x.SalonId == salonId && x.Email != null && x.Email.ToLower() == email.ToLower());

    public async Task<IEnumerable<Customer>> GetCustomersAsync(Guid salonId, string? firstName = null,
        string? lastName = null, string? phoneNumber = null, string? email = null, bool? isActive = null)
    {
        var query = _context.Customers.AsQueryable()
            .Where(x => x.SalonId == salonId);
        
        if (isActive != null)
            query = query.Where(x => x.IsActive == isActive);
        
        if (!string.IsNullOrWhiteSpace(firstName))
            query = query.Where(x => x.FirstName.ToLower().Contains(firstName.ToLower()));

        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(x => x.LastName.ToLower().Contains(lastName.ToLower()));

        if (!string.IsNullOrWhiteSpace(phoneNumber))
            query = query.Where(x => x.PhoneNumber.Contains(phoneNumber));

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(x => x.Email != null && x.Email.ToLower().Contains(email.ToLower()));

        return await query.ToListAsync();
    }
}