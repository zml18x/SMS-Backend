using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Payment.Validators;

public class CreateAppointmentPaymentRequestValidator : AbstractValidator<CreateAppointmentPaymentRequest>
{
    public CreateAppointmentPaymentRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .ValidateId("CustomerId");
        
        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("Payment Date is required.")
            .Must(date => date <= DateTime.Now).WithMessage("Payment Date cannot be in the future.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Payment method must be a valid enum value.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.")
            .LessThanOrEqualTo(100000000).WithMessage("Amount must not exceed 100000000.");
        
        When(x => x.Notes != null, () =>
        {
            RuleFor(x => x.Notes!)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        });
    }
}