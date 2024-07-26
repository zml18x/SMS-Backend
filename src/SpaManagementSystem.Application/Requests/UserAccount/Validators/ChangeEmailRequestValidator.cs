using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators;

/// <summary>
/// Validator class for validating <see cref="ChangeEmailRequest"/> instances.
/// </summary>
public class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequest>
{
    public ChangeEmailRequestValidator()
    {
        RuleFor(x => x.NewEmail)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
    }
}