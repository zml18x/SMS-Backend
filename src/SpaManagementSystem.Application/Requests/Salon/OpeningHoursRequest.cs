namespace SpaManagementSystem.Application.Requests.Salon;

/// <summary>
/// Represents a request to define or update the opening hours for a specific day of the week.
/// </summary>
public record OpeningHoursRequest(DayOfWeek DayOfWeek, TimeSpan OpeningTime, TimeSpan ClosingTime);