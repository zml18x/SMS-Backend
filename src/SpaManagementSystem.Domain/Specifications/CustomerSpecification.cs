using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class CustomerSpecification : ISpecification<Customer>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(Customer entity)
    {
        SpecificationHelper.ValidateGuid(entity.SalonId, _result, "SalonId is required (Cannot be Guid.Empty).");
        SpecificationHelper.ValidateString(entity.FirstName, _result, "FirstName is required.");
        SpecificationHelper.ValidateString(entity.LastName, _result, "LastName is required.");
        SpecificationHelper.ValidateGender(entity.Gender, _result);
        if (!string.IsNullOrWhiteSpace(entity.Email))
            SpecificationHelper.ValidateEmail(entity.Email, _result);
        SpecificationHelper.ValidatePhoneNumber(entity.PhoneNumber, _result);
        SpecificationHelper.ValidateOptionalStringLength(entity.Notes, 500, _result,
            "Notes cannot be longer than 500 characters.");

        return _result;
    }
}