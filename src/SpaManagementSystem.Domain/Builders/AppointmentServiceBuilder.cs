using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.Builders;

public class AppointmentServiceBuilder(ISpecification<AppointmentService> specification) :  IBuilder<AppointmentService>
{
    private Guid _id = Guid.Empty;
    private Guid _salonId = Guid.Empty;
    private Guid _appointmentId = Guid.Empty;
    private Guid _serviceId = Guid.Empty;
    private decimal _price;
    
    public AppointmentService Build()
    {
        var appointmentService = new AppointmentService(_id, _salonId, _appointmentId, _serviceId, _price);
        
        var validationResult = specification.IsSatisfiedBy(appointmentService);
        if (!validationResult.IsValid)
            throw new DomainValidationException($"appointment service creation failed: {string.Join(", ", validationResult.Errors)}");

        return appointmentService;
    }
    
    public AppointmentServiceBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public AppointmentServiceBuilder WithSalonId(Guid salonId)
    {
        _salonId = salonId;
        return this;
    }

    public AppointmentServiceBuilder WithAppointmentId(Guid appointmentId)
    {
        _appointmentId = appointmentId;
        return this;
    }

    public AppointmentServiceBuilder WithServiceId(Guid serviceId)
    {
        _serviceId = serviceId;
        return this;
    }

    public AppointmentServiceBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }
}