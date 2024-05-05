using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Provides validation for <see cref="ResetPasswordRequest"/> using FluentValidation.
    /// This class ensures that the data provided for resetting a password is valid and complete,
    /// enforcing correctness before the password reset process is initiated.
    /// </summary>
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordRequestValidator"/> class
        /// and defines the rules for validating the request.
        /// </summary>
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.NewPassword)
                .MatchPassword();
            
            RuleFor(x => x.Token)
                .NotNull().WithMessage("Confirmation token is required")
                .NotEmpty().WithMessage("Confirmation token is required");
        }
    }
}