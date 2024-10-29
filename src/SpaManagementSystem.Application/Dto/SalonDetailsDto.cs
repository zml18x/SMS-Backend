namespace SpaManagementSystem.Application.Dto;

public record SalonDetailsDto(
    Guid Id,
    string Name,
    string Email,
    string PhoneNumber,
    string? Description,
    AddressDto? Address,
    IEnumerable<OpeningHoursDto> OpeningHours);