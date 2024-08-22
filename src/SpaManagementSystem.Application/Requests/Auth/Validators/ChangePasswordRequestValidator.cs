using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Auth.Validators;

/// <summary>
/// Validator class for validating <see cref="ChangePasswordRequest"/> instances.
/// </summary>
public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .MatchPassword();
            
        RuleFor(x => x.NewPassword)
            .MatchPassword();
    }
}