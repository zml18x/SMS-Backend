using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Dto;

/// <summary>
/// Data Transfer Object (DTO) for representing a user profile.
/// </summary>
public record UserProfileDto(string FirstName, string LastName, GenderType Gender, DateOnly DateOfBirth);