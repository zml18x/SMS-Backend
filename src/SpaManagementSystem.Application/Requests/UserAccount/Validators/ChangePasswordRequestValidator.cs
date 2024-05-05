using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Provides validation for <see cref="ChangePasswordRequest"/> using FluentValidation.
    /// This class ensures that the passwords provided for changing a user's password are valid according to defined security rules,
    /// enforcing the necessary security standards before allowing a password change.
    /// </summary>
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordRequestValidator"/> class
        /// and defines the rules for validating the request.
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