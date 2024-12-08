using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Payment;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Interfaces;

public interface IPaymentService
{
    public Task<PaymentDto> CreateAppointmentPaymentAsync(Guid appointmentId, CreateAppointmentPaymentRequest request);
    public Task<PaymentDto> GetByIdAsync(Guid paymentId);
    public Task<IEnumerable<PaymentDto>> GetPaymentsForAppointmentAsync(Guid appointmentId);
    public Task<IEnumerable<PaymentDto>> GetPaymentsForCustomerAsync(Guid customerId, DateOnly? startDate, DateOnly? endDate);
    public Task UpdatePaymentStatusAsync(Guid paymentId, UpdatePaymentStatusRequest request);
}