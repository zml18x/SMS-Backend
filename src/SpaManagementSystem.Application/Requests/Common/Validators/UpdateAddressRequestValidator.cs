using FluentValidation;

namespace SpaManagementSystem.Application.Requests.Common.Validators;

public class UpdateAddressRequestValidator : AbstractValidator<UpdateAddressRequest>
{
    public UpdateAddressRequestValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .Length(2, 50).WithMessage("Country must be between 2 and 100 characters.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .Length(2, 50).WithMessage("City must be between 2 and 100 characters.");

        RuleFor(x => x.Region)
            .NotEmpty().WithMessage("Region is required.")
            .Length(2, 50).WithMessage("Region must be between 2 and 100 characters.");

        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("PostalCode is required.")
            .Length(3, 10).WithMessage("Postal code must be between 3 and 10 characters.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .Length(2, 100).WithMessage("Street must be between 2 and 100 characters.");

        RuleFor(x => x.BuildingNumber)
            .NotEmpty().WithMessage("BuildingNumber is required.")
            .Length(1, 10).WithMessage("BuildingNumber must be between 1 and 10 characters.");
    }
}