using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Auth.Validators;

/// <summary>
/// Validator class for validating <see cref="UserRegisterRequest"/> instances.
/// </summary>
public class RegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .MatchPassword();

        RuleFor(x => x.PhoneNumber)
            .MatchPhoneNumber();
    }
}