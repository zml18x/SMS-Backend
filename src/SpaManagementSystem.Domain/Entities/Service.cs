using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class Service : BaseEntity
{
    public string Name { get; protected set; }
    public string Code { get; protected set; }
    public string? Description { get; protected set; }
    public decimal Price { get; protected set; }
    public decimal TaxRate { get; protected set; }
    public TimeSpan Duration { get; protected set; }
    public string? ImgUrl { get; protected set; }
    public bool IsActive { get; protected set; }
    
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; }
    
    public decimal PriceWithTax => Price + Price * TaxRate;
    
    
    
    protected Service(){}

    public Service(Guid id, Guid salonId, string name, string code, string? description,
        decimal price, decimal taxRate, TimeSpan duration, string? imgUrl)
    {
        Id = id;
        SalonId = salonId;
        Name = name;
        Code = code;
        Description = description;
        Price = price;
        TaxRate = taxRate;
        Duration = duration;
        ImgUrl = imgUrl;
        IsActive = true;
    }
    
    
    public bool UpdateService(string name, string code, string? description, decimal price, decimal taxRate, 
        TimeSpan duration, string? imgUrl, bool isActive)
    {
        var anyDataUpdated = false;
        
        if (!string.IsNullOrWhiteSpace(name))
        {
            Name = name;
            anyDataUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(code))
        {
            Code = code;
            anyDataUpdated = true;
        }
        
        if (Description != description)
        {
            Description = description;
            anyDataUpdated = true;
        }

        if (Price != price)
        {
            Price = price;
            anyDataUpdated = true;
        }
        
        if (TaxRate != taxRate)
        {
            TaxRate = taxRate;
            anyDataUpdated = true;
        }

        if (Duration != duration)
        {
            Duration = duration;
            anyDataUpdated = true;
        }
        
        if (ImgUrl != imgUrl)
        {
            ImgUrl = imgUrl;
            anyDataUpdated = true;
        }

        if (IsActive != isActive)
        {
            IsActive = isActive;
            anyDataUpdated = true;
        }
        
        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }
}