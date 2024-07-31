using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;
using SpaManagementSystem.Application.Exceptions;

namespace SpaManagementSystem.Infrastructure.Repositories;

/// <summary>
/// Represents a repository for managing salon.
/// This repository implements both the general repository operations provided through the Repository base class
/// and the specific operations defined in the <see cref="ISalonRepository"/>.
/// </summary>
public class SalonRepository : Repository<Salon>, ISalonRepository
{
    private readonly SmsDbContext _context;


    
    /// <summary>
    /// Initializes a new instance of the <see cref="SalonRepository"/> class with the specified context.
    /// </summary>
    /// <param name="context">The database context used for salon data operations.</param>
    public SalonRepository(SmsDbContext context) : base(context)
    {
        _context = context;
    }



    /// <inheritdoc />
    /// <exception cref="NotFoundException">Thrown when no salon is found for the specified <paramref name="userId"/>.</exception>
    public async Task<Salon> GetByUserIdAsync(Guid userId)
    {
        var salon = await _context.Salons.FirstOrDefaultAsync(x => x.UserId == userId);
        if (salon == null)
            throw new NotFoundException($"Salon for user id {userId} was not found.");
    
        return salon;
    }
        
    /// <inheritdoc />
    public async Task<IEnumerable<Salon>> GetAllByUserIdAsync(Guid userId)
        => await Task.FromResult(await _context.Salons.Where(x => x.UserId == userId).ToListAsync());
}