using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="ChangeEmailRequest"/> instances.
    /// </summary>
    public class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmailRequestValidator"/> class.
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