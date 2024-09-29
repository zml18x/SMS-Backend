using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Auth.Validators;

public class RegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .MatchEmail();

        RuleFor(x => x.Password)
            .MatchPassword();

        RuleFor(x => x.PhoneNumber)
            .MatchPhoneNumber();
    }
}