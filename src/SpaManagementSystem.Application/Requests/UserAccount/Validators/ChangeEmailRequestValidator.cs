using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Provides validation for <see cref="ChangeEmailRequest"/> using FluentValidation.
    /// This class ensures that the new email provided in the request is valid and not empty,
    /// enforcing correctness before the email change is processed.
    /// </summary>
    public class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmailRequestValidator"/> class
        /// and defines the rules for validating the request.
        /// </summary>
        public ChangeEmailRequestValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");
        }
    }
}