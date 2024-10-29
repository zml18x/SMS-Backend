using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.Builders;

public class ProductBuilder(ISpecification<Product> specification) : IBuilder<Product>
{
#nullable disable
    private Guid _id = Guid.Empty;
    private Guid _salonId;
    private string _name;
    private string _code;
    private decimal _purchasePrice;
    private decimal _purchaseTaxRate;
    private decimal _salePrice;
    private decimal _saleTaxRate;
    private decimal _stockQuantity;
    private int _minimumStockQuantity;
    private string _unitOfMeasure;
#nullable enable
    private string? _description;
    private string? _imgUrl;
    
    
    
    public Product Build()
    {
        var product = new Product(_id, _salonId, _name, _code, _description, _purchasePrice,
            _purchaseTaxRate, _salePrice, _saleTaxRate, _stockQuantity, _minimumStockQuantity, _unitOfMeasure, _imgUrl);

        var validationResult = specification.IsSatisfiedBy(product);
        
        if (!validationResult.IsValid)
            throw new DomainValidationException($"Product creation failed: {string.Join(", ", validationResult.Errors)}");

        return product;
    }

    public ProductBuilder WithProductId(Guid productId)
    {
        _id = productId;
        return this;
    }

    public ProductBuilder WithSalonId(Guid salonId)
    {
        _salonId = salonId;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public ProductBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public ProductBuilder WithPurchasePrice(decimal purchasePrice)
    {
        _purchasePrice = purchasePrice;
        return this;
    }
    
    public ProductBuilder WithPurchaseTaxRate(decimal purchaseTaxRate)
    {
        _purchaseTaxRate = purchaseTaxRate;
        return this;
    }
    
    public ProductBuilder WithSalePrice(decimal salePrice)
    {
        _salePrice = salePrice;
        return this;
    }
    
    public ProductBuilder WithSaleTaxRate(decimal saleTaxRate)
    {
        _saleTaxRate = saleTaxRate;
        return this;
    }

    public ProductBuilder WithStockQuantity(decimal stockQuantity)
    {
        _stockQuantity = stockQuantity;
        return this;
    }

    public ProductBuilder WithMinimumStockQuantity(int minimumStockQuantity)
    {
        _minimumStockQuantity = minimumStockQuantity;
        return this;
    }

    public ProductBuilder WithUnitOfMeasure(string unitOfMeasure)
    {
        _unitOfMeasure = unitOfMeasure;
        return this;
    }

    public ProductBuilder WithImgUrl(string? imgUrl)
    {
        _imgUrl = imgUrl;
        return this;
    }
}