using FluentValidation;

namespace SpaManagementSystem.Application.Requests.Auth.Validators;

/// <summary>
/// Validator class for validating <see cref="SignInRequest"/> instances.
/// </summary>
public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("Password is required.")
            .NotEmpty().WithMessage("Password is required");
    }
}