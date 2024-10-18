using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class Service : BaseEntity
{
    private ISet<Resource> _resources = new HashSet<Resource>(); //usage ?
    private ISet<ServiceProductUsage> _productsUsages = new HashSet<ServiceProductUsage>();
    public string Name { get; protected set; }
    public string Code { get; protected set; }
    public string? Description { get; protected set; }
    public decimal Price { get; protected set; }
    public decimal TaxRate { get; protected set; }
    public TimeSpan Duration { get; protected set; }
    public string? ImgUrl { get; protected set; }
    public bool IsActive { get; protected set; }
    public Guid CreatedByEmployeeId { get; protected set; }
    public Guid UpdatedByEmployeeId { get; protected set; }
    
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; }
    
    public IEnumerable<Resource> Resources => _resources;
    public IEnumerable<ServiceProductUsage> ProductUsages => _productsUsages;
    public decimal PriceWithTax => Price + Price * TaxRate;
    
    
    
    protected Service(){}

    public Service(Guid id, Guid salonId, Guid createdByEmployeeId, string name, string code, string? description,
        decimal price, decimal taxRate, TimeSpan duration, string? imgUrl)
    {
        Id = id;
        SalonId = salonId;
        CreatedByEmployeeId = createdByEmployeeId;
        UpdatedByEmployeeId = createdByEmployeeId;
        Name = name;
        Code = code;
        Description = description;
        Price = price;
        TaxRate = taxRate;
        Duration = duration;
        ImgUrl = imgUrl;
        IsActive = true;
    }
}