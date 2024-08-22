using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Interfaces;

public interface ITokenService
{
    public JwtDto CreateJwtToken(UserDto user);
}