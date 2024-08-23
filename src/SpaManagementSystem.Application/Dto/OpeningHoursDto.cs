namespace SpaManagementSystem.Application.Dto;

public record OpeningHoursDto(DayOfWeek DayOfWeek, TimeSpan OpeningTime, TimeSpan ClosingTime);