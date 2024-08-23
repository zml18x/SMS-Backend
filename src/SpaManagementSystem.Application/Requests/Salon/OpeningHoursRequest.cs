namespace SpaManagementSystem.Application.Requests.Salon;

public record OpeningHoursRequest(DayOfWeek DayOfWeek, TimeSpan OpeningTime, TimeSpan ClosingTime);