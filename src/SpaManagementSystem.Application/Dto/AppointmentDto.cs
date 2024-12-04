using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Dto;

public record AppointmentDto(
    Guid Id,
    Guid SalonId,
    Guid EmployeeId,
    Guid CustomerId,
    DateOnly Date,
    DateTime StartTime,
    DateTime EndTime,
    AppointmentStatus Status,
    decimal TotalPrice,
    bool IsFullyPaid,
    IEnumerable<AppointmentServiceDto> AppointmentServices,
    IEnumerable<PaymentDto> Payments,
    string? Notes);