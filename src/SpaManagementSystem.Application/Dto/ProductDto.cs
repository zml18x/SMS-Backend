namespace SpaManagementSystem.Application.Dto;

public record ProductDto(
    Guid Id,
    Guid SalonId,
    string Name,
    string Code,
    string? Description,
    decimal PurchasePrice,
    decimal PurchaseTaxRate,
    decimal SalePrice,
    decimal SaleTaxRate,
    decimal StockQuantity,
    decimal PurchasePriceWithTax,
    decimal SalePriceWithTax,
    int MinimumStockQuantity,
    string UnitOfMeasure,
    string? ImgUrl);