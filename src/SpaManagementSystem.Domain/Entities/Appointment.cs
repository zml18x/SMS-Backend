using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.Common.Helpers;

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
    public bool CanUpdate => EnsureStatusAllowsAction(AppointmentStatus.Pending, AppointmentStatus.Confirmed);
    public bool CanDelete => EnsureStatusAllowsAction(AppointmentStatus.Pending, AppointmentStatus.Confirmed) && !_payments.Any();
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



    public void AddService(AppointmentService service)
    {
        if (!CanUpdate)
            throw new InvalidOperationException($"Cannot add services when appointment status is {Status}.");
        
        _appointmentServices.Add(service);
        UpdateTimestamp();
    }

    public void RemoveService(AppointmentService service)
    {
        if (!CanUpdate)
            throw new InvalidOperationException($"Cannot remove services when appointment status is {Status}.");
        
        _appointmentServices.Remove(service);
        UpdateTimestamp();
    }
    
    public void ChangeStatus(AppointmentStatus newStatus)
    {
        if (!IsValidStatusTransition(Status, newStatus))
            throw new InvalidOperationException($"Cannot change status from {Status} to {newStatus}.");

        Status = newStatus;
        UpdateTimestamp();
    }

    public bool UpdateAppointment(Guid employeeId, DateOnly date, DateTime startTime, DateTime endTime, string? notes = null)
    {
        if (!CanUpdate)
            throw new InvalidOperationException($"Cannot update appointment when status is {Status}.");
        
        var propertyChanges = new Dictionary<Action, bool>
        {
            { () => EmployeeId = employeeId, EmployeeId != employeeId },
            { () => Date = date, Date != date },
            { () => StartTime = startTime, StartTime != startTime },
            { () => EndTime = endTime, EndTime != endTime },
            { () => Notes = notes, Notes != notes },
        };

        var anyDataUpdated = EntityUpdater.ApplyChanges(propertyChanges);

        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }

    private bool IsValidStatusTransition(AppointmentStatus currentStatus, AppointmentStatus newStatus)
    {
        return (currentStatus, newStatus) switch
        {
            (AppointmentStatus.Pending, AppointmentStatus.Confirmed) => true,
            (AppointmentStatus.Pending, AppointmentStatus.Canceled) => true,
            (AppointmentStatus.Confirmed, AppointmentStatus.Completed) => true,
            (AppointmentStatus.Confirmed, AppointmentStatus.Canceled) => true,
            (AppointmentStatus.Confirmed, AppointmentStatus.NoShow) => true,
            _ => false
        };
    }
    
    private bool EnsureStatusAllowsAction(params AppointmentStatus[] allowedStatuses)
    {
        return allowedStatuses.Contains(Status);
    }
}