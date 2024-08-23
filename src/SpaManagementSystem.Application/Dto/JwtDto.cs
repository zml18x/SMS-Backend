namespace SpaManagementSystem.Application.Dto;

public record JwtDto(string Token, DateTime ExpirationTime);