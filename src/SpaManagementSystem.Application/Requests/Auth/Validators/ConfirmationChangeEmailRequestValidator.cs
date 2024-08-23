using FluentValidation;

namespace SpaManagementSystem.Application.Requests.Auth.Validators;

public class ConfirmationChangeEmailRequestValidator : AbstractValidator<ConfirmationChangeEmailRequest>
{
    public ConfirmationChangeEmailRequestValidator()
    {
        RuleFor(x => x.NewEmail)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        
        RuleFor(x => x.Token)
            .NotNull().WithMessage("Confirmation token is required")
            .NotEmpty().WithMessage("Confirmation token is required");
    }
}