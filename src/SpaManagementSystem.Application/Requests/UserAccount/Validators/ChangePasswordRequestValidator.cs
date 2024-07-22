using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="ChangePasswordRequest"/> instances.
    /// </summary>
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordRequestValidator"/> class.
        /// </summary>
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .MatchPassword();
            
            RuleFor(x => x.NewPassword)
                .MatchPassword();
        }
    }
}