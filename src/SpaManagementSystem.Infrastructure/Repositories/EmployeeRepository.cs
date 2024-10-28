using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class EmployeeRepository(SmsDbContext context) : Repository<Employee>(context), IEmployeeRepository, IUniqueCodeRepository
{
    private readonly SmsDbContext _context = context;

    
    
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

    public async Task<Employee?> GetByCodeAsync(string employeeCode)
        => await _context.Employees.FirstOrDefaultAsync(x => x.Code.ToUpper() == employeeCode.ToUpper());
    
    public async Task<Employee?> GetWithProfileByCodeAsync(string employeeCode)
        => await _context.Employees
            .Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.Code.ToUpper() == employeeCode.ToUpper());

    public async Task<bool> IsExistsAsync(Guid salonId, string code)
        => await _context.Employees.AnyAsync(x => x.SalonId == salonId && x.Code.ToUpper() == code.ToUpper());
}