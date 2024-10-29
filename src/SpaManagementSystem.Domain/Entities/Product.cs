using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.Common.Helpers;

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
    public int MinimumStockQuantity { get; protected set; }
    public string UnitOfMeasure { get; protected set; }
    public bool IsActive { get; protected set; }
    public string? ImgUrl { get; protected set; }
    
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; }
    
    public decimal PurchasePriceWithTax => PurchasePrice + PurchasePrice * PurchaseTaxRate;
    public decimal SalePriceWithTax => SalePrice + SalePrice * SaleTaxRate;
    
    
    
    protected Product(){}
    
    public Product(Guid id, Guid salonId, string name, string code, string? description,
        decimal purchasePrice, decimal purchaseTaxRate, decimal salePrice, decimal saleTaxRate, decimal stockQuantity,
        int minimumStockQuantity, string unitOfMeasure, string? imgUrl)
    {
        Id = id;
        SalonId = salonId;
        Name = name;
        Code = code;
        Description = description;
        PurchasePrice = purchasePrice;
        PurchaseTaxRate = purchaseTaxRate;
        SalePrice = salePrice;
        SaleTaxRate = saleTaxRate;
        StockQuantity = stockQuantity;
        MinimumStockQuantity = minimumStockQuantity;
        UnitOfMeasure = unitOfMeasure;
        ImgUrl = imgUrl;
        IsActive = true;
    }
    
    
    
    public bool UpdateProduct(string name, string code, string? description, decimal purchasePrice, decimal purchaseTaxRate, 
        decimal salePrice, decimal saleTaxRate, decimal stockQuantity, int minimumStockQuantity, string unitOfMeasure,
        bool isActive, string? imgUrl)
    {
        var propertyChanges = new Dictionary<Action, bool>
        {
            { () => Name = name, Name != name },
            { () => Code = code, Code != code },
            { () => Description = description, Description != description },
            { () => PurchasePrice = purchasePrice, PurchasePrice != purchasePrice },
            { () => PurchaseTaxRate = purchaseTaxRate, PurchaseTaxRate != purchaseTaxRate },
            { () => SalePrice = salePrice, SalePrice != salePrice },
            { () => SaleTaxRate = saleTaxRate, SaleTaxRate != saleTaxRate },
            { () => StockQuantity = stockQuantity, StockQuantity != stockQuantity },
            { () => MinimumStockQuantity = minimumStockQuantity, MinimumStockQuantity != minimumStockQuantity },
            { () => UnitOfMeasure = unitOfMeasure, UnitOfMeasure != unitOfMeasure },
            { () => ImgUrl = imgUrl, ImgUrl != imgUrl },
            { () => IsActive = isActive, IsActive != isActive }
        };

        var anyDataUpdated = EntityUpdater.ApplyChanges(propertyChanges);

        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }
}