using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Service.Validators;

public class UpdateServiceRequestValidator : AbstractValidator<UpdateServiceRequest>
{
    public UpdateServiceRequestValidator()
    {
        RuleFor(x => x.Name)
            .MatchName();
        
        RuleFor(x => x.Code)
            .MatchCode();
        
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description!)
                .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters");
        });

        RuleFor(x => x.Price)
            .ValidatePrice();

        RuleFor(x => x.TaxRate)
            .ValidateTaxRate();

        RuleFor(x => x.Duration)
            .ValidateServiceDuration();

        When(x => !string.IsNullOrEmpty(x.ImgUrl), () =>
        {
            RuleFor(x => x.ImgUrl)
                .ValidateUrl();
        });

        RuleFor(x => x.IsActive)
            .Must(isActive => isActive || !isActive)
            .WithMessage("IsActive must be either true or false.");
    }
}