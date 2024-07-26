namespace SpaManagementSystem.Application.Dto;

/// <summary>
/// Data Transfer Object (DTO) for representing a JSON Web Token (JWT) and its expiration time.
/// </summary>
public record JwtDto(string Token, DateTime Expire);