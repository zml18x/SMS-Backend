namespace SpaManagementSystem.Application.Dto;

/// <summary>
/// Data Transfer Object (DTO) for representing detailed information about a salon.
/// </summary>
public record SalonDetailsDto(Guid Id, string Name, string Email, string PhoneNumber, string? Description,
    AddressDto? Address, IEnumerable<OpeningHoursDto> OpeningHours);