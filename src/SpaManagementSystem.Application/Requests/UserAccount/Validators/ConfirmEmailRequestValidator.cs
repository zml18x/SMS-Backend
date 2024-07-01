using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator for <see cref="ConfirmEmailRequest"/> using FluentValidation.
    /// This class defines validation rules to ensure that the data provided for email confirmation is correct and complete.
    /// </summary>
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConfirmEmailRequestValidator"/> and sets up the validation rules for confirming an email.
        /// </summary>
        public ConfirmEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Token)
                .NotNull().WithMessage("Confirmation token is required")
                .NotEmpty().WithMessage("Confirmation token is required");
        }
    }
}