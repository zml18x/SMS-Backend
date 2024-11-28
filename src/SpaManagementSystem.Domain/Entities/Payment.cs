using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; } = null!;
    
    public Guid AppointmentId { get; protected set; }
    public Appointment Appointment { get; protected set; } = null!;
    
    public DateTime PaymentDate { get; protected set; }
    public PaymentStatus Status { get; protected set; }
    public PaymentMethod Method { get; protected set; }
    public decimal Amount { get; protected set; }
    public string? Notes { get; protected set; }
    
    
    
    protected Payment(){}

    public Payment(Guid id, Guid salonId, Guid appointmentId, DateTime paymentDate, PaymentStatus status,
        PaymentMethod method, decimal amount, string? notes = null)
    {
        Id = id;
        SalonId = salonId;
        AppointmentId = appointmentId;
        PaymentDate = paymentDate;
        Status = status;
        Method = method;
        Amount = amount;
        Notes = notes;
    }
}