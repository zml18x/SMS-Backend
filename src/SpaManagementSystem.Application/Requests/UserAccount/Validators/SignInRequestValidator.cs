using FluentValidation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="SignInRequest"/> instances.
    /// </summary>
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignInRequestValidator"/> class.
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