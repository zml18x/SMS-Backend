using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="ConfirmEmailRequest"/> instances.
    /// </summary>
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmEmailRequestValidator"/> class.
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