using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class Employee : BaseEntity
{
    public Guid SalonId { get; protected set; }
    public Guid UserId { get; protected set; }
    public string Position { get; protected set; }
    public string Code { get; protected set; }
    public Salon Salon { get; protected set; }
    
    
        
    public Employee(Guid id, Guid salonId, Guid userId, string position, string code) : base (id)
    {
        SalonId = salonId;
        UserId = userId;
        Position = position;
        Code = code;
    }
}