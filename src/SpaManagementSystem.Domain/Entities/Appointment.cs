using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities;

public class Appointment : BaseEntity
{
    private ISet<AppointmentService> _appointmentServices = new HashSet<AppointmentService>();
    private ISet<Payment> _payments = new HashSet<Payment>();
    
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; } = null!;
    
    public Guid EmployeeId { get; protected set; }
    public Employee Employee { get; protected set; } = null!;
    
    public Guid CustomerId { get; protected set; }
    public Customer Customer { get; protected set; } = null!;
    
    public DateOnly Date { get; protected set; }
    public DateTime StartTime { get; protected set; }
    public DateTime EndTime { get; protected set; }
    public AppointmentStatus Status { get; protected set; }
    public string? Notes { get; protected set; }
    public decimal TotalPrice => _appointmentServices.Sum(x => x.Price);
    public bool IsFullyPaid => _payments.Sum(p => p.Amount) >= TotalPrice;
    public IEnumerable<AppointmentService> AppointmentServices => _appointmentServices;
    public IEnumerable<Payment> Payments => _payments;
    
    
    
    protected Appointment(){}

    public Appointment(Guid id, Guid salonId, Guid employeeId, Guid customerId, DateOnly date, DateTime startTime,
        DateTime endTime, string? notes = null)
    {
        Id = id;
        SalonId = salonId;
        EmployeeId = employeeId;
        CustomerId = customerId;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
        Notes = notes;
        Status = AppointmentStatus.Pending;
    }
}