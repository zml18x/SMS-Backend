using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;

namespace SpaManagementSystem.Application.Services;

public class RefreshTokenService(IRefreshTokenRepository repository) : IRefreshTokenService
{
    public async Task<bool> IsValidToken(Guid userId, string refreshToken)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.");
        
        var storedRefreshToken = await repository.GetByUserId(userId);

        return storedRefreshToken != null && storedRefreshToken.Token == refreshToken &&
               storedRefreshToken.ExpirationTime > DateTime.UtcNow;
    }

    public async Task SaveRefreshToken(RefreshTokenDto refreshToken)
    {
        if (refreshToken.UserId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.");
        
        var storedRefreshToken = await repository.GetByUserId(refreshToken.UserId);
        
        if (storedRefreshToken != null)
            repository.Delete(storedRefreshToken);
        
        await repository.CreateAsync(new RefreshToken(Guid.NewGuid(), refreshToken.UserId, refreshToken.Token,
            refreshToken.ExpirationTime));
            
        await repository.SaveChangesAsync();
    }
}