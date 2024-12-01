using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.Builders;

public class PaymentBuilder(ISpecification<Payment> specification) : IBuilder<Payment>
{
    private Guid _id = Guid.Empty;
    private Guid _salonId = Guid.Empty;
    private Guid _appointmentId = Guid.Empty;
    private DateTime _paymentDate;
    private PaymentStatus _status;
    private PaymentMethod _method;
    private decimal _amount;
    private string? _notes;
    
    public Payment Build()
    {
        var payment = new Payment(_id, _salonId, _appointmentId, _paymentDate, _status, _method, _amount, _notes);
        
        var validationResult = specification.IsSatisfiedBy(payment);
        if (!validationResult.IsValid)
            throw new DomainValidationException($"Payment creation failed: {string.Join(", ", validationResult.Errors)}");
        
        return payment;
    }
    
    public PaymentBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public PaymentBuilder WithSalonId(Guid salonId)
    {
        _salonId = salonId;
        return this;
    }

    public PaymentBuilder WithAppointmentId(Guid appointmentId)
    {
        _appointmentId = appointmentId;
        return this;
    }

    public PaymentBuilder WithPaymentDate(DateTime paymentDate)
    {
        _paymentDate = paymentDate;
        return this;
    }

    public PaymentBuilder WithStatus(PaymentStatus status)
    {
        _status = status;
        return this;
    }

    public PaymentBuilder WithMethod(PaymentMethod method)
    {
        _method = method;
        return this;
    }

    public PaymentBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public PaymentBuilder WithNotes(string? notes)
    {
        _notes = notes;
        return this;
    }
}