namespace SpaManagementSystem.Application.Requests.EmployeeAvailability;

public record AvailabilityHoursRequest(TimeSpan Start, TimeSpan End);