using System.Security.Claims;
using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Interfaces;

public interface ITokenService
{
    public JwtDto CreateJwtToken(UserDto user);
    public RefreshTokenDto CreateRefreshToken(Guid userId);
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}