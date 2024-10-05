using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Salon.Validators;

public class UpdateSalonRequestValidator : AbstractValidator<UpdateSalonRequest>
{
    public UpdateSalonRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Salon name cannot be empty.")
            .MinimumLength(2).WithMessage("Salon name must be at least 2 characters long.")
            .MaximumLength(30).WithMessage("Salon name cannot be longer than 30 characters")
            .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Salon name can only contain letters, numbers, and spaces");
        
        RuleFor(x => x.PhoneNumber)
            .MatchPhoneNumber();

        RuleFor(x => x.Email)
            .MatchEmail();
        
        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description!)
                .MaximumLength(1000).WithMessage("Description cannot be longer than 1000 characters");
        });
    }
}