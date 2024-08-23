namespace SpaManagementSystem.Application.Dto;

public record RefreshTokenDto(Guid UserId, string Token, DateTime ExpirationTime);