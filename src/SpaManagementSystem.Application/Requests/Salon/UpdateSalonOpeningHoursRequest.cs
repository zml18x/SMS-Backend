using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Requests.Salon;

/// <summary>
/// Represents a request to update the opening hours of an existing salon.
/// </summary>
public record UpdateSalonOpeningHoursRequest(List<OpeningHoursDto> OpeningHours);