using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class ProductSpecification : ISpecification<Product>
{
    private readonly ValidationResult _result = new(true);

    public ValidationResult IsSatisfiedBy(Product entity)
    {
        ValidateSalonId(entity.SalonId);
        ValidateCreatedByEmployeeId(entity.CreatedByEmployeeId);
        ValidateName(entity.Name);
        ValidateCode(entity.Code);
        ValidateDescription(entity.Description);
        ValidatePurchasePrice(entity.PurchasePrice);
        ValidatePurchaseTaxRate(entity.PurchaseTaxRate);
        ValidateSalePrice(entity.SalePrice);
        ValidateSaleTaxRate(entity.SaleTaxRate);
        ValidateStockQuantity(entity.StockQuantity);
        ValidateMinimumStockLevel(entity.MinimumStockLevel);
        ValidateUnitOfMeasure(entity.UnitOfMeasure);
        ValidateImgUrl(entity.ImgUrl);
        
        return _result;
    }
    
    private void ValidateSalonId(Guid salonId)
    {
        if (salonId == Guid.Empty)
            _result.AddError("SalonId is required (Cannot be Guid.Empty).");
    }
    
    private void ValidateCreatedByEmployeeId(Guid createdByEmployeeId)
    {
        if (createdByEmployeeId == Guid.Empty)
            _result.AddError("EmployeeId is required (Cannot be Guid.Empty).");
    }
    
    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            _result.AddError("Product name is required.");
    }
    
    private void ValidateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            _result.AddError("Code is required.");
    }
    
    private void ValidateDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description) && description.Length > 500)
            _result.AddError("Product description cannot be longer than 500 characters.");
    }
    
    private void ValidatePurchasePrice(decimal purchasePrice)
    {
        if (purchasePrice < 0)
            _result.AddError("Purchase price cannot be negative.");
    }

    private void ValidatePurchaseTaxRate(decimal purchaseTaxRate)
    {
        if (purchaseTaxRate < 0 || purchaseTaxRate > 1)
            _result.AddError("Purchase tax rate must be between 0 and 1.");
    }

    private void ValidateSalePrice(decimal salePrice)
    {
        if (salePrice < 0)
            _result.AddError("Sale price cannot be negative.");
    }

    private void ValidateSaleTaxRate(decimal saleTaxRate)
    {
        if (saleTaxRate < 0 || saleTaxRate > 1)
            _result.AddError("Sale tax rate must be between 0 and 1.");
    }

    private void ValidateStockQuantity(decimal stockQuantity)
    {
        if (stockQuantity < 0)
            _result.AddError("Stock quantity cannot be negative.");
    }

    private void ValidateMinimumStockLevel(int minimumStockLevel)
    {
        if (minimumStockLevel < 0)
            _result.AddError("Minimum stock level cannot be negative.");
    }

    private void ValidateUnitOfMeasure(string unitOfMeasure)
    {
        if (string.IsNullOrWhiteSpace(unitOfMeasure))
            _result.AddError("Unit of measure is required.");
    }

    private void ValidateImgUrl(string? imgUrl)
    {
        if (!string.IsNullOrWhiteSpace(imgUrl) && !Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute))
            _result.AddError("Image URL is not valid.");
    }
}