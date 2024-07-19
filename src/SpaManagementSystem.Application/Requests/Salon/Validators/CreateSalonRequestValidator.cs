using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Salon.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="CreateSalonRequest"/> instances.
    /// </summary>
    public class CreateSalonRequestValidator : AbstractValidator<CreateSalonRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSalonRequestValidator"/> class.
        /// </summary>
        public CreateSalonRequestValidator()
        {
            // Validate the salon name
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Salon name cannot be empty.")
                .MinimumLength(2).WithMessage("Salon name must be at least 2 characters long.")
                .MaximumLength(30).WithMessage("Salon name cannot be longer than 30 characters")
                .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Salon name can only contain letters, numbers, and spaces");
            
            // Validate the salon phone number
            RuleFor(x => x.PhoneNumber)
                .MatchPhoneNumber();
            
            // Validate the salon email
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");
        }
    }
}