using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Entities;

public class EmployeeAvailability : BaseEntity
{
    private readonly ISet<AvailableHours> _availableHours = new HashSet<AvailableHours>();
    public Guid EmployeeId { get; protected set; }
    public DateOnly Date { get; protected set; }
    public IEnumerable<AvailableHours> AvailableHours => _availableHours;
    public Employee Employee { get; protected set; }



    public EmployeeAvailability(Guid id, Guid employeeId, DateOnly date)
    {
        Id = id;
        EmployeeId = employeeId;
        Date = date;
    }
    
    public EmployeeAvailability(Guid id, Guid employeeId, DateOnly date, IEnumerable<AvailableHours> availableHours)
    {
        Id = id;
        EmployeeId = employeeId;
        Date = date;

        if (availableHours == null)
            throw new DomainValidationException("Available hours cannot be null.");
        
        availableHours.ToList().ForEach(AddAvailableHour);
    }
    
    
    
    public void AddAvailableHour(AvailableHours availableHour)
    {
        if (_availableHours.Any(existing => existing.Overlaps(availableHour)))
            throw new DomainValidationException("Cannot add overlapping hours.");

        _availableHours.Add(availableHour);
    }

    public void ClearAvailableHours()
    {
        _availableHours.Clear();
    }
}