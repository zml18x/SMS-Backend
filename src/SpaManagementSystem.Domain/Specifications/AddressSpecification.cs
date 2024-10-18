using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Specifications;

public class AddressSpecification : ISpecification<Address>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(Address entity)
    {
        SpecificationHelper.ValidateString(entity.Country, _result, "Country is required.");
        SpecificationHelper.ValidateString(entity.Region, _result, "Region is required.");
        SpecificationHelper.ValidateString(entity.City, _result, "City is required.");
        SpecificationHelper.ValidateString(entity.PostalCode, _result, "PostalCode is required.");
        SpecificationHelper.ValidateString(entity.Street, _result, "Street is required.");
        SpecificationHelper.ValidateString(entity.BuildingNumber, _result, "BuildingNumber is required.");
        
        return _result;
    }
}