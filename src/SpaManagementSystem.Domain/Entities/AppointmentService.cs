using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class AppointmentService : BaseEntity
{
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; } = null!;
    
    public Guid AppointmentId { get; protected set; }
    public Appointment Appointment { get; protected set; } = null!;

    public Guid ServiceId { get; protected set; }
    public Service Service { get; protected set; } = null!;

    public decimal Price { get; protected set; }
    
    
    
    protected AppointmentService() { }
    
    public AppointmentService(Guid id, Guid salonId, Guid appointmentId, Guid serviceId, decimal price)
    {
        Id = id;
        SalonId = salonId;
        AppointmentId = appointmentId;
        ServiceId = serviceId;
        Price = price;
    }
}