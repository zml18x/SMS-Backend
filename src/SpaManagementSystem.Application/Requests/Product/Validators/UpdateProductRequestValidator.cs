using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Product.Validators;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
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

        RuleFor(x => x.PurchasePrice)
            .ValidatePrice();

        RuleFor(x => x.PurchaseTaxRate)
            .ValidateTaxRate();
        
        RuleFor(x => x.SalePrice)
            .ValidatePrice();

        RuleFor(x => x.SaleTaxRate)
            .ValidateTaxRate();

        RuleFor(product => product.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.")
            .WithMessage("Stock quantity should be a positive value.");

        RuleFor(product => product.MinimumStockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum stock quantity cannot be negative.");
        
        RuleFor(product => product.UnitOfMeasure)
            .NotEmpty().WithMessage("Unit of measure cannot be empty.")
            .Must(unit => unit.Length <= 6).WithMessage("Unit of measure cannot exceed 6 characters.")
            .Matches(@"^[.a-zA-Z]+$").WithMessage("Unit of measure must contain only letters and dots.");

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