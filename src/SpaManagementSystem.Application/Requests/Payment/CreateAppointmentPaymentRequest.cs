using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.Payment;

public record CreateAppointmentPaymentRequest(
    Guid CustomerId,
    DateTime PaymentDate,
    PaymentMethod PaymentMethod,
    decimal Amount,
    string? Notes);