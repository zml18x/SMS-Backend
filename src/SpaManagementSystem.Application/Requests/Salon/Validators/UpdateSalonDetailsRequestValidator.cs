using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Salon.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="UpdateSalonDetailsRequest"/> instances.
    /// </summary>
    public class UpdateSalonDetailsRequestValidator : AbstractValidator<UpdateSalonDetailsRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSalonDetailsRequestValidator"/> class.
        /// </summary>
        public UpdateSalonDetailsRequestValidator()
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
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");
            
            // Validate the salon description
            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description!)
                    .MaximumLength(1000).WithMessage("Description cannot be longer than 1000 characters");
            });
        }
    }
}