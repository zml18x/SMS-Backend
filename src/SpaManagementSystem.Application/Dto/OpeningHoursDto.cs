namespace SpaManagementSystem.Application.Dto;

/// <summary>
/// Data Transfer Object (DTO) for representing the opening hours of a salon for a specific day.
/// </summary>
public record OpeningHoursDto(DayOfWeek DayOfWeek, TimeSpan OpeningTime, TimeSpan ClosingTime);