using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="SendConfirmationEmailRequest"/> instances.
    /// </summary>
    public class SendConfirmationEmailRequestValidator : AbstractValidator<SendConfirmationEmailRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendConfirmationEmailRequestValidator"/> class.
        /// </summary>
        public SendConfirmationEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");
        }
    }
}