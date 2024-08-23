using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Interfaces;

public interface IRefreshTokenService
{
    public Task<bool> IsValidToken(Guid userId, string refreshToken);
    public Task SaveRefreshToken(RefreshTokenDto refreshToken);
}