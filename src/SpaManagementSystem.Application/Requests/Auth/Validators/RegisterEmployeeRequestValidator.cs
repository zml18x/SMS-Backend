using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Auth.Validators;

public class RegisterEmployeeRequestValidator : AbstractValidator<RegisterEmployeeRequest>
{
    public RegisterEmployeeRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        
        RuleFor(x => x.PhoneNumber)
            .MatchPhoneNumber();
    }
}