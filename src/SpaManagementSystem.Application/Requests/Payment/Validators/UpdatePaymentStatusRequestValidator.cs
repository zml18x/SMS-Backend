using FluentValidation;

namespace SpaManagementSystem.Application.Requests.Payment.Validators;

public class UpdatePaymentStatusRequestValidator : AbstractValidator<UpdatePaymentStatusRequest>
{
    public UpdatePaymentStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Payment status must be a valid enum value.");
    }
}