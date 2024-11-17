using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Builders;

public class EmployeeAvailabilityBuilder(ISpecification<EmployeeAvailability> specification) : IBuilder<EmployeeAvailability>
{
    private Guid _id = Guid.Empty;
    private Guid _employeeId;
    private DateOnly _date;
    private IEnumerable<AvailableHours>? _availableHours;
    
    
    
    public EmployeeAvailability Build()
    {
        var employeeAvailability = _availableHours != null
            ? new EmployeeAvailability(_id, _employeeId, _date, _availableHours)
            : new EmployeeAvailability(_id, _employeeId, _date);
        
        var validationResult = specification.IsSatisfiedBy(employeeAvailability);
        if (!validationResult.IsValid)
            throw new DomainValidationException($"Employee availability creation failed: {string.Join(", ", validationResult.Errors)}");

        return employeeAvailability;
    }
    
    public EmployeeAvailabilityBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }
    
    public EmployeeAvailabilityBuilder WithEmployeeId(Guid employeeId)
    {
        _employeeId = employeeId;
        return this;
    }

    public EmployeeAvailabilityBuilder WithDate(DateOnly date)
    {
        _date = date;
        return this;
    }

    public EmployeeAvailabilityBuilder WithAvailableHours(IEnumerable<AvailableHours> availableHours)
    {
        _availableHours = availableHours;
        return this;
    }
}