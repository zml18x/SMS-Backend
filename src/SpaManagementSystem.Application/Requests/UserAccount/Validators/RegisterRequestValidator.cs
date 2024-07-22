using FluentValidation;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="RegisterRequest"/> instances.
    /// </summary>
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterRequestValidator"/> class.
        /// </summary>
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .MatchPassword();

            RuleFor(x => x.PhoneNumber)
                .MatchPhoneNumber();

            RuleFor(x => x.FirstName)
                .NotNull().WithMessage("First name is required.")
                .NotEmpty().WithMessage("First name is required.")
                .MatchName()
                .Length(2, 50).WithMessage("The first name can be 2 characters long and up to 50 characters long");

            RuleFor(x => x.LastName)
                .NotNull().WithMessage("Last name is required.")
                .NotEmpty().WithMessage("Last name is required.")
                .MatchName()
                .Length(2, 50).WithMessage("The last name can be 2 characters long and up to 50 characters long");
            
            RuleFor(x => x.DateOfBirth)
                .NotNull().WithMessage("Date of birth is required")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Date of birth must be less than or equal to today's date");

            RuleFor(x => x.Gender)
                .NotNull().WithMessage("Gender is required.")
                .NotEmpty().WithMessage("Gender is required")
                .IsEnumName(typeof(GenderType), false).WithMessage("Gender must be either 'male', 'female', or 'other'.");
        }
    }
}