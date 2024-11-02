using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.Common.Helpers;

namespace SpaManagementSystem.Domain.Entities;

public class Service : BaseEntity
{
    private ISet<Employee> _employees = new HashSet<Employee>();
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
    public IEnumerable<Employee> Employees => _employees;
    
    
    
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
        var propertyChanges = new Dictionary<Action, bool>
        {
            { () => Name = name, Name != name },
            { () => Code = code, Code != code },
            { () => Description = description, Description != description },
            { () => Price = price, Price != price },
            { () => TaxRate = taxRate, TaxRate != taxRate },
            { () => Duration = duration, Duration != duration },
            { () => ImgUrl = imgUrl, ImgUrl != imgUrl },
            { () => IsActive = isActive, IsActive != isActive }
        };

        var anyDataUpdated = EntityUpdater.ApplyChanges(propertyChanges);

        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }
}