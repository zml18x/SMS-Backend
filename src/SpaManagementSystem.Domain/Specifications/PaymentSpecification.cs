using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class PaymentSpecification : ISpecification<Payment>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(Payment entity)
    {
        SpecificationHelper.ValidateGuid(entity.SalonId, _result, "SalonId is required (Cannot be Guid.Empty");
        SpecificationHelper.ValidateGuid(entity.AppointmentId, _result, "AppointmentId is required (Cannot be Guid.Empty");
        
        ValidatePaymentStatus(entity.Status);
        ValidatePaymentMethod(entity.Method);
        SpecificationHelper.ValidatePrice(entity.Amount, _result, "Amount is required (Cannot be Zero or Negative)", false);
        SpecificationHelper.ValidateOptionalStringLength(entity.Notes, 500,
            _result, "Notes cannot be longer than 500 characters.");
        
        return _result;
    }

    private void ValidatePaymentDate(DateTime paymentDate)
    {
        var now = DateTime.UtcNow;

        if (paymentDate > now)
            _result.AddError("Payment date cannot be in the future.");
    }

    private void ValidatePaymentStatus(PaymentStatus status)
    {
        if (!Enum.IsDefined(typeof(PaymentStatus), status))
            _result.AddError($"Invalid payment status: {status}");
    }

    private void ValidatePaymentMethod(PaymentMethod paymentMethod)
    {
        if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
            _result.AddError($"Invalid payment method: {paymentMethod}");
    }
}