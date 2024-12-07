using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class PaymentRepository(SmsDbContext context) : Repository<Payment>(context), IPaymentRepository
{
    private readonly SmsDbContext _context = context;
    
    public async Task<IEnumerable<Payment>> GetPaymentsForCustomerAsync(Guid customerId, DateOnly? startDate = null, DateOnly? endDate = null)
    {
        var query = _context.Payments.AsQueryable();

        query = query.Where(p => p.CustomerId == customerId);

        if (startDate.HasValue)
            query = query.Where(p => DateOnly.FromDateTime(p.PaymentDate) >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => DateOnly.FromDateTime(p.PaymentDate) <= endDate.Value);
        
        query = query.OrderByDescending(p => p.PaymentDate);
        
        return await query.ToListAsync();
    }
}