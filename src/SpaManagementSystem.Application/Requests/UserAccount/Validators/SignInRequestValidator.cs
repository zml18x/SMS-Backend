using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator for <see cref="SignInRequest"/> using FluentValidation.
    /// This class defines validation rules to ensure that user credentials provided
    /// during the sign-in process are correctly formatted and not empty.
    /// </summary>
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SignInRequestValidator"/> and sets up the validation rules for signing in.
        /// </summary>
        public SignInRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password is required.")
                .NotEmpty().WithMessage("Password is required");
        }
    }
}