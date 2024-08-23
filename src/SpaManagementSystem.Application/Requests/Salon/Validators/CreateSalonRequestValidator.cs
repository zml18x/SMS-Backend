using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Salon.Validators;

public class CreateSalonRequestValidator : AbstractValidator<CreateSalonRequest>
{
    public CreateSalonRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Salon name cannot be empty.")
            .MinimumLength(2).WithMessage("Salon name must be at least 2 characters long.")
            .MaximumLength(30).WithMessage("Salon name cannot be longer than 30 characters")
            .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Salon name can only contain letters, numbers, and spaces");
        
        RuleFor(x => x.PhoneNumber)
            .MatchPhoneNumber();
        
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
    }
}