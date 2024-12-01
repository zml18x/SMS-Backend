using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class AppointmentServiceSpecification : ISpecification<AppointmentService>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(AppointmentService entity)
    {
        SpecificationHelper.ValidateGuid(entity.SalonId, _result, "SalonId is required (Cannot be Guid.Empty");
        SpecificationHelper.ValidateGuid(entity.AppointmentId, _result, "AppointmentId is required (Cannot be Guid.Empty");
        SpecificationHelper.ValidateGuid(entity.ServiceId, _result, "ServiceId is required (Cannot be Guid.Empty");
        SpecificationHelper.ValidatePrice(entity.Price, _result, "Price cannot be negative.");
        
        return _result;
    }
}