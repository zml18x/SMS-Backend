namespace SpaManagementSystem.Application.Requests.Product;

public record CreateProductRequest(
    Guid SalonId,
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
    string? ImgUrl);