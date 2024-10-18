using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class Resource : BaseEntity
{
    public string Name { get; protected set; }
    public string Code { get; protected set; }
    public string? Description { get; protected set; }
    public int Quantity { get; protected set; }
    public bool IsActive { get; protected set; }
    public string? ImgUrl { get; protected set; }
    public Guid CreatedByEmployeeId { get; protected set; }
    public Guid UpdatedByEmployeeId { get; protected set; }
    
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; }
    
    
    
    protected Resource(){}
    
    public Resource(Guid id, Guid salonId, Guid createdByEmployeeId, string name, string code, string? description,
        int quantity, string? imgUrl)
    {
        Id = id;
        SalonId = salonId;
        CreatedByEmployeeId = createdByEmployeeId;
        UpdatedByEmployeeId = createdByEmployeeId;
        Name = name;
        Code = code;
        Description = description;
        Quantity = quantity;
        ImgUrl = imgUrl;
        IsActive = true;
    }
}