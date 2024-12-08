using AutoMapper;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Payment;

namespace SpaManagementSystem.Application.Services;

public class PaymentService(
    IPaymentRepository paymentRepository,
    IAppointmentRepository appointmentRepository,
    ICustomerRepository customerRepository,
    PaymentBuilder paymentBuilder,
    IMapper mapper) : IPaymentService
{
    public async Task<PaymentDto> CreateAppointmentPaymentAsync(Guid appointmentId, CreateAppointmentPaymentRequest request)
    {
        var appointment = await appointmentRepository.GetOrThrowAsync(() => appointmentRepository.GetByIdAsync(appointmentId));

        if (!appointment.CanBePaid)
            throw new InvalidOperationException($"Appointment with status {appointment.Status} cannot be paid.");
        
        await customerRepository.GetOrThrowAsync(() => customerRepository.GetByIdAsync(request.CustomerId));
        
        var payment = paymentBuilder
            .WithSalonId(appointment.SalonId)
            .WithAppointmentId(appointmentId)
            .WithCustomerId(request.CustomerId)
            .WithPaymentDate(request.PaymentDate)
            .WithStatus(PaymentStatus.Pending)
            .WithMethod(request.PaymentMethod)
            .WithAmount(request.Amount)
            .WithNotes(request.Notes)
            .Build();

        appointment.AddPayment(payment);

        await appointmentRepository.SaveChangesAsync();
        
        return mapper.Map<PaymentDto>(payment);
    }

    public async Task<PaymentDto> GetByIdAsync(Guid paymentId)
    {
        var payment = await paymentRepository.GetOrThrowAsync(() => paymentRepository.GetByIdAsync(paymentId));
        
        return mapper.Map<PaymentDto>(payment);
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsForAppointmentAsync(Guid appointmentId)
    {
        var appointment = await appointmentRepository.GetOrThrowAsync(() => appointmentRepository.GetByIdAsync(appointmentId));

        return mapper.Map<IEnumerable<PaymentDto>>(appointment.Payments);
    }

    public async Task<IEnumerable<PaymentDto>> GetPaymentsForCustomerAsync(Guid customerId, DateOnly? startDate, DateOnly? endDate)
    {
        await customerRepository.GetOrThrowAsync(() => customerRepository.GetByIdAsync(customerId));

        var payments = await paymentRepository.GetPaymentsForCustomerAsync(customerId, startDate, endDate);
        
        return mapper.Map<IEnumerable<PaymentDto>>(payments);
    }

    public async Task UpdatePaymentStatusAsync(Guid paymentId, UpdatePaymentStatusRequest request)
    {
        var payment = await paymentRepository.GetOrThrowAsync(() => paymentRepository.GetByIdAsync(paymentId));
        
        payment.ChangeStatus(request.Status);
        
        await paymentRepository.SaveChangesAsync();
    }
}