namespace SpaManagementSystem.Application.Dto;

public record AppointmentServiceDto(
    Guid Id,
    Guid SalonId,
    Guid AppointmentId,
    Guid ServiceId,
    decimal Price);