using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class EmployeeRepository(SmsDbContext context) : Repository<Employee>(context), IEmployeeRepository, IUniqueCodeRepository
{
    private readonly SmsDbContext _context = context;

    
    
    public async Task<bool> IsExistsAsync(Guid salonId, string code)
        => await _context.Employees.AnyAsync(x => x.SalonId == salonId && x.Code.ToUpper() == code.ToUpper());
    
    public async Task<Employee?> GetByUserIdAsync(Guid userId)
        => await _context.Employees.FirstOrDefaultAsync(x => x.UserId == userId);

    public async Task<Employee?> GetWithProfileByUserIdAsync(Guid userId)
        => await _context.Employees
            .Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.UserId == userId);

    public async Task<Employee?> GetWithProfileByIdAsync(Guid employeeId)
        => await _context.Employees
            .Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.Id == employeeId);
    
    public async Task<Employee?> GetWithServicesByIdAsync(Guid employeeId)
        => await _context.Employees
            .Include(x => x.Services)
            .FirstOrDefaultAsync(x => x.Id == employeeId);
    
    public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid salonId, string? code = null,
        string? firstName = null, string? lastName = null, EmploymentStatus? status = null)
    {
        var query = _context.Employees.Include(x => x.Profile)
            .AsQueryable().Where(x => x.SalonId == salonId);
        
        if (status != null)
            query = query.Where(x => x.EmploymentStatus == status);
        
        if (!string.IsNullOrEmpty(firstName))
            query = query.Where(x => x.Profile.FirstName.ToLower().Contains(firstName.ToLower()));
        
        if (!string.IsNullOrEmpty(lastName))
            query = query.Where(x => x.Profile.LastName.ToLower().Contains(lastName.ToLower()));

        if (!string.IsNullOrEmpty(code))
            query = query.Where(x => x.Code.ToLower().Contains(code.ToLower()));

        return await query.ToListAsync();
    }
}