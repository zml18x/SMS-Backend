using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Provides validation for <see cref="SendResetPasswordTokenRequest"/> using FluentValidation.
    /// This class ensures that the data provided for sending a password reset token is valid and complete,
    /// enforcing correctness before the password reset process is initiated.
    /// </summary>
    public class SendResetPasswordTokenRequestValidator : AbstractValidator<SendResetPasswordTokenRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendResetPasswordTokenRequestValidator"/> class
        /// and defines the rules for validating the request.
        /// </summary>
        public SendResetPasswordTokenRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.NewPassword)
                .MatchPassword();
        }
    }
}