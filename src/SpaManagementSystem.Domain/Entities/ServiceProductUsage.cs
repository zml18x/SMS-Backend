using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class ServiceProductUsage : BaseEntity
{
    public Guid ServiceId { get; protected set; }
    public Service Service { get; protected set; }

    public Guid ProductId { get; protected set; }
    public Product Product { get; protected set; }

    public decimal QuantityUsed { get; protected set; }
    
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; }
    
    
    
    protected ServiceProductUsage(){}

    public ServiceProductUsage(Guid id, Guid salonId, Guid serviceId, Guid productId, decimal quantityUsed)
    {
        Id = id;
        SalonId = salonId;
        ServiceId = serviceId;
        ProductId = productId;
        QuantityUsed = quantityUsed;
    }
}