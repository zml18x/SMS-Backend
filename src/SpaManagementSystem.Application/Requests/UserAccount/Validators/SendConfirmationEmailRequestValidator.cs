using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Provides validation for <see cref="SendConfirmationEmailRequest"/> using FluentValidation.
    /// This class ensures that the email provided in the request is valid and not empty,
    /// enforcing correctness before a confirmation email is sent.
    /// </summary>
    public class SendConfirmationEmailRequestValidator : AbstractValidator<SendConfirmationEmailRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendConfirmationEmailRequestValidator"/> class
        /// and defines the rules for validating the request.
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