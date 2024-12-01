using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.Builders;

public class AppointmentBuilder(ISpecification<Appointment> specification) : IBuilder<Appointment>
{
    private Guid _id = Guid.Empty;
    private Guid _salonId = Guid.Empty;
    private Guid _employeeId = Guid.Empty;
    private Guid _customerId = Guid.Empty;
    private DateOnly _date;
    private DateTime _startTime;
    private DateTime _endTime;
    private string? _notes;
    
    
    public Appointment Build()
    {
        var appointment = new Appointment(_id, _salonId, _employeeId, _customerId, _date, _startTime, _endTime, _notes);
        
        var validationResult = specification.IsSatisfiedBy(appointment);
        if (!validationResult.IsValid)
            throw new DomainValidationException($"Appointment creation failed: {string.Join(", ", validationResult.Errors)}");

        return appointment;
    }

    public AppointmentBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public AppointmentBuilder WithSalonId(Guid salonId)
    {
        _salonId = salonId;
        return this;
    }

    public AppointmentBuilder WithEmployeeId(Guid employeeId)
    {
        _employeeId = employeeId;
        return this;
    }

    public AppointmentBuilder WithCustomerId(Guid customerId)
    {
        _customerId = customerId;
        return this;
    }

    public AppointmentBuilder WithDate(DateOnly date)
    {
        _date = date;
        return this;
    }

    public AppointmentBuilder WithStartTime(DateTime startTime)
    {
        _startTime = startTime;
        return this;
    }

    public AppointmentBuilder WithEndTime(DateTime endTime)
    {
        _endTime = endTime;
        return this;
    }

    public AppointmentBuilder WithNotes(string? notes)
    {
        _notes = notes;
        return this;
    }
}