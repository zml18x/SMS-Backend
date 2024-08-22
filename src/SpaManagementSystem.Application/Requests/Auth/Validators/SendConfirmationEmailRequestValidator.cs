using FluentValidation;

namespace SpaManagementSystem.Application.Requests.Auth.Validators;

/// <summary>
/// Validator class for validating <see cref="SendConfirmationEmailRequest"/> instances.
/// </summary>
public class SendConfirmationEmailRequestValidator : AbstractValidator<SendConfirmationEmailRequest>
{
    public SendConfirmationEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
    }
}