using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.Payment;

public record UpdatePaymentStatusRequest(PaymentStatus Status);