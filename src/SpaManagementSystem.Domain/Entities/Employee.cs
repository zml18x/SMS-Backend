using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.Common.Helpers;

namespace SpaManagementSystem.Domain.Entities;

public class Employee : BaseEntity
{
    private ISet<Service> _services = new HashSet<Service>();
    private ISet<EmployeeAvailability> _employeeAvailabilities = new HashSet<EmployeeAvailability>();
    private ISet<Appointment> _appointments = new HashSet<Appointment>();
    public Guid SalonId { get; protected set; }
    public Guid UserId { get; protected set; }
    public string Position { get; protected set; }
    public EmploymentStatus EmploymentStatus { get; protected set; }
    public string Code { get; protected set; }
    public string Color { get; protected set; }
    public DateOnly HireDate { get; protected set; }
    public string? Notes { get; protected set; }
    public Salon Salon { get; protected set; }
    public EmployeeProfile Profile { get; protected set; }
    public IEnumerable<Service> Services => _services;
    public IEnumerable<EmployeeAvailability> EmployeeAvailabilities => _employeeAvailabilities;
    public IEnumerable<Appointment> Appointments => _appointments;
    
    
    
    protected Employee(){}

    public Employee(Guid id, Guid salonId, Guid userId, string position, EmploymentStatus employmentStatus, string code,
        DateOnly hireDate, string color, string? notes)
    {
        Id = id;
        SalonId = salonId;
        UserId = userId;
        Position = position;
        EmploymentStatus = employmentStatus;
        Code = code;
        HireDate = hireDate;
        Color = color;
        Notes = notes;
    }


    
    public void AddEmployeeProfile(EmployeeProfile employeeProfile)
    {
        Profile = employeeProfile;
    }

    public bool UpdateEmployee(string color, string? notes)
    {
        var propertyChanges = new Dictionary<Action, bool>
        {
            { () => Color = color, Color != color },
            { () => Notes = notes, Notes != notes },
        };

        var anyDataUpdated = EntityUpdater.ApplyChanges(propertyChanges);

        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }
    
    public bool UpdateEmployee(string position, EmploymentStatus employmentStatus, string code, string color,
        DateOnly hireDate, string? notes)
    {
        var propertyChanges = new Dictionary<Action, bool>
        {
            { () => Position = position, Position != position },
            { () => EmploymentStatus = employmentStatus, EmploymentStatus != employmentStatus },
            { () => Code = code, Code != code },
            { () => Color = color, Color != color },
            { () => HireDate = hireDate, HireDate != hireDate },
            { () => Notes = notes, Notes != notes },
        };

        var anyDataUpdated = EntityUpdater.ApplyChanges(propertyChanges);

        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }

    public void AddService(Service service)
    {
        _services.Add(service);
    }

    public void RemoveService(Service service)
    {
        _services.Remove(service);
    }
    
    public void AddAvailability(EmployeeAvailability employeeAvailability)
    {
        _employeeAvailabilities.Add(employeeAvailability);
    }

    public bool HasAvailabilityOnDate(DateOnly date)
        => EmployeeAvailabilities.Any(a => a.Date == date);
}