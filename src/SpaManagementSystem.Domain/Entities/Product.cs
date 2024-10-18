using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; protected set; }
    public string Code { get; protected set; }
    public string? Description { get; protected set; }
    public decimal PurchasePrice { get; protected set; }
    public decimal SalePrice { get; protected set; }
    public decimal PurchaseTaxRate { get; protected set; }
    public decimal SaleTaxRate { get; protected set; }
    public decimal StockQuantity { get; protected set; }
    public int MinimumStockLevel { get; protected set; }
    public string UnitOfMeasure { get; protected set; }
    public bool IsActive { get; protected set; }
    public string? ImgUrl { get; protected set; }
    public Guid CreatedByEmployeeId { get; protected set; }
    public Guid UpdatedByEmployeeId { get; protected set; }
    
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; }
    
    public decimal PurchasePriceWithTax => PurchasePrice + PurchasePrice * PurchaseTaxRate;
    public decimal SellingPriceWithTax => SalePrice + SalePrice * SaleTaxRate;
    
    
    
    protected Product(){}
    
    public Product(Guid id, Guid salonId, Guid createdByEmployeeId, string name, string code, string? description,
        decimal purchasePrice, decimal purchaseTaxRate, decimal salePrice, decimal saleTaxRate, decimal stockQuantity,
        int minimumStockLevel, string unitOfMeasure, string? imgUrl)
    {
        Id = id;
        SalonId = salonId;
        CreatedByEmployeeId = createdByEmployeeId;
        UpdatedByEmployeeId = createdByEmployeeId;
        Name = name;
        Code = code;
        Description = description;
        PurchasePrice = purchasePrice;
        PurchaseTaxRate = purchaseTaxRate;
        SalePrice = salePrice;
        SaleTaxRate = saleTaxRate;
        StockQuantity = stockQuantity;
        MinimumStockLevel = minimumStockLevel;
        UnitOfMeasure = unitOfMeasure;
        ImgUrl = imgUrl;
        IsActive = true;
    }
}