using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class SalonSpecification : ISpecification<Salon>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(Salon entity)
    {
        SpecificationHelper.ValidateGuid(entity.UserId, _result, "UserId is required (Cannot be Guid.Empty).");
        SpecificationHelper.ValidateString(entity.Name, _result, "Salon name is required.");
        SpecificationHelper.ValidatePhoneNumber(entity.PhoneNumber, _result);
        SpecificationHelper.ValidateEmail(entity.Email, _result);
        SpecificationHelper.ValidateOptionalStringLength(entity.Description, 1000,
            _result, "Product description cannot be longer than 1000 characters.");
        
        return _result;
    }
}