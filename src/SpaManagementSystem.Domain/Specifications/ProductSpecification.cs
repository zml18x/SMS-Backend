using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class ProductSpecification : ISpecification<Product>
{
    private readonly ValidationResult _result = new(true);

    public ValidationResult IsSatisfiedBy(Product entity)
    {
        SpecificationHelper.ValidateGuid(entity.SalonId, _result, "SalonId is required (Cannot be Guid.Empty).");
        SpecificationHelper.ValidateString(entity.Name, _result, "Product name is required.");
        SpecificationHelper.ValidateString(entity.Code, _result, "Product code is required.");
        SpecificationHelper.ValidateOptionalStringLength(entity.Description, 500,
            _result, "Product description cannot be longer than 500 characters.");
        SpecificationHelper.ValidatePrice(entity.PurchasePrice, _result, "Purchase price cannot be negative.");
        SpecificationHelper.ValidateTaxRate(entity.PurchaseTaxRate, _result, "Purchase tax rate must be between 0 and 1.");
        SpecificationHelper.ValidatePrice(entity.SalePrice, _result, "Sale price cannot be negative.");
        SpecificationHelper.ValidateTaxRate(entity.SaleTaxRate, _result, "Sale tax rate must be between 0 and 1.");
        SpecificationHelper.ValidateQuantity(entity.StockQuantity, _result, "Stock quantity cannot be negative");
        SpecificationHelper.ValidateQuantity(entity.MinimumStockQuantity, _result, "Minimum stock quantity cannot be negative.");
        SpecificationHelper.ValidateString(entity.UnitOfMeasure, _result, "Unit of measure is required.");
        SpecificationHelper.ValidateOptionalUrl(entity.ImgUrl, _result, "Image URL is not valid.");
        
        return _result;
    }
}