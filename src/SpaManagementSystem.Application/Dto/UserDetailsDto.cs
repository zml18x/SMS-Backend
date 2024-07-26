namespace SpaManagementSystem.Application.Dto;

/// <summary>
/// Data Transfer Object (DTO) for representing a User Details.
/// </summary>
public record UserDetailsDto(Guid UserId, string Email, string PhoneNumber, string FirstName, string LastName,
    string Gender, DateOnly DateOfBirth);
