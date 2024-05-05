using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Provides validation for <see cref="ConfirmationChangeEmailRequest"/> using FluentValidation.
    /// This class ensures that the data provided for email change confirmation is correct and complete,
    /// enforcing correctness before the email change confirmation process is executed.
    /// </summary>
    public class ConfirmationChangeEmailRequestValidator : AbstractValidator<ConfirmationChangeEmailRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationChangeEmailRequestValidator"/> class
        /// and defines the rules for validating the request.
        /// </summary>
        public ConfirmationChangeEmailRequestValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Token)
                .NotNull().WithMessage("Confirmation token is required")
                .NotEmpty().WithMessage("Confirmation token is required");
        }
    }
}