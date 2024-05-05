using FluentValidation;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator for <see cref="RegisterRequest"/> using FluentValidation.
    /// This class defines validation rules for registering a new user, ensuring that all necessary information is provided and valid.
    /// </summary>
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RegisterRequestValidator"/> and defines the rules for validation.
        /// </summary>
        public RegisterRequestValidator()
        {
            RuleFor(r => r.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(r => r.Password)
                .MatchPassword();

            RuleFor(r => r.PhoneNumber)
                .MatchPhoneNumber();

            RuleFor(r => r.FirstName)
                .NotNull().WithMessage("First name is required.")
                .NotEmpty().WithMessage("First name is required.")
                .MatchName()
                .Length(2, 50).WithMessage("The first name can be 2 characters long and up to 50 characters long");

            RuleFor(r => r.LastName)
                .NotNull().WithMessage("Last name is required.")
                .NotEmpty().WithMessage("Last name is required.")
                .MatchName()
                .Length(2, 50).WithMessage("The last name can be 2 characters long and up to 50 characters long");
            
            RuleFor(r => r.DateOfBirth)
                .NotNull().WithMessage("Date of birth is required")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Date of birth must be less than or equal to today's date");

            RuleFor(r => r.Gender)
                .NotNull().WithMessage("Gender is required.")
                .NotEmpty().WithMessage("Gender is required")
                .IsEnumName(typeof(GenderType), false).WithMessage("Gender must be either 'male', 'female', or 'other'.");
        }
    }
}