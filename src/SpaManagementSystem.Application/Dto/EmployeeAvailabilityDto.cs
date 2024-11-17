namespace SpaManagementSystem.Application.Dto;

public record EmployeeAvailabilityDto(
    Guid Id,
    Guid EmployeeId,
    DateOnly Date,
    IEnumerable<AvailableHoursDto> AvailableHours);