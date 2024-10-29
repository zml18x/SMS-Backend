namespace SpaManagementSystem.Application.Requests.Product;

public record UpdateProductRequest(
    string Name,
    string Code,
    string? Description,
    decimal PurchasePrice,
    decimal PurchaseTaxRate,
    decimal SalePrice,
    decimal SaleTaxRate,
    decimal StockQuantity,
    int MinimumStockQuantity,
    string UnitOfMeasure,
    bool IsActive,
    string? ImgUrl);