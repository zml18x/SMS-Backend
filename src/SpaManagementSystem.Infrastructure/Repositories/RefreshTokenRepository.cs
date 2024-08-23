using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class RefreshTokenRepository(SmsDbContext context) : Repository<RefreshToken>(context), IRefreshTokenRepository
{
    private readonly SmsDbContext _context = context;

    

    public async Task<RefreshToken?> GetByUserId(Guid userId)
        => await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
}