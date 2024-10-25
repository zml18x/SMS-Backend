using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;
using SpaManagementSystem.Application.Exceptions;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class SalonRepository(SmsDbContext context) : Repository<Salon>(context), ISalonRepository
{
    private readonly SmsDbContext _context = context;


    public async Task<Salon> GetByUserIdAsync(Guid userId)
    {
        var salon = await _context.Salons.FirstOrDefaultAsync(x => x.UserId == userId);
        if (salon == null)
            throw new NotFoundException($"Salon for user id {userId} was not found.");
    
        return salon;
    }

    public async Task<Salon?> GetWithEmployeesByIdAsync(Guid salonId)
        => await context.Salons.Include(x => x.Employees).FirstOrDefaultAsync(x => x.Id == salonId);

    public async Task<IEnumerable<Salon>> GetAllByUserIdAsync(Guid userId)
        => await _context.Salons.Where(x => x.UserId == userId).ToListAsync();

    public async Task<Salon?> GetWithServicesAsync(Guid salonId)
        => await _context.Salons.Include(x => x.Services).FirstOrDefaultAsync(x => x.Id == salonId);
}