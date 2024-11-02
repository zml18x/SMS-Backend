using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class ServiceRepository(SmsDbContext context) : Repository<Service>(context), IServiceRepository
{
    private readonly SmsDbContext _context = context;
    
    
    
    public async Task<bool> IsExistsAsync(Guid salonId, string code)
        => await _context.Services.AnyAsync(x => x.SalonId == salonId && x.Code.ToUpper() == code.ToUpper());
    
    public async Task<Service?> GetWithEmployeesAsync(Guid serviceId)
        => await _context.Services.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Id == serviceId);

    public async Task<IEnumerable<Service>> GetServicesAsync(Guid salonId, string? code = null, string? name = null,
        bool? active = null)
    {
        var query = _context.Services.AsQueryable()
            .Where(x => x.SalonId == salonId);
        
        if (active != null)
            query = query.Where(x => x.IsActive == active);
        
        if (!string.IsNullOrEmpty(name))
            query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));

        if (!string.IsNullOrEmpty(code))
            query = query.Where(x => x.Code.ToLower().Contains(code.ToLower()));

        return await query.ToListAsync();
    }
}