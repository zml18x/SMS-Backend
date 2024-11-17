namespace SpaManagementSystem.Application.Requests.EmployeeAvailability;

public record EmployeeAvailabilityRequest(
    DateOnly Date,
    IEnumerable<AvailabilityHoursRequest> AvailabilityHours);