using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="ConfirmationChangeEmailRequest"/> instances.
    /// </summary>
    public class ConfirmationChangeEmailRequestValidator : AbstractValidator<ConfirmationChangeEmailRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationChangeEmailRequestValidator"/> class.
        /// </summary>
        public ConfirmationChangeEmailRequestValidator()
        {
            // Validation email
            RuleFor(x => x.NewEmail)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");
            
            // Validation token
            RuleFor(x => x.Token)
                .NotNull().WithMessage("Confirmation token is required")
                .NotEmpty().WithMessage("Confirmation token is required");
        }
    }
}