using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Dto;

public record PaymentDto(
    Guid Id,
    Guid SalonId,
    Guid AppointmentId,
    DateTime PaymentDate,
    PaymentStatus Status,
    PaymentMethod Method,
    decimal Amount,
    string? Notes);