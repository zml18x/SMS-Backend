using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Customer.Validators;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(x => x.SalonId)
            .NotEmpty().WithMessage("SalonId is required.")
            .Must(g => g != Guid.Empty).WithMessage("SalonId must be a valid non-empty GUID.");
        
        RuleFor(x => x.FirstName)
            .MatchFirstName();
        
        RuleFor(x => x.LastName)
            .MatchLastName();
        
        RuleFor(x => x.Gender)
            .MatchGender();
        
        RuleFor(x => x.PhoneNumber)
            .MatchPhoneNumber();

        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email!)
                .MatchEmail();
        });
        
        When(x => !string.IsNullOrEmpty(x.Notes), () =>
        {
            RuleFor(x => x.Notes!)
                .MaximumLength(500).WithMessage("Notes cannot be longer than 500 characters");
        });
    }
}