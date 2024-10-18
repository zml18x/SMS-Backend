using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class ResourceSpecification : ISpecification<Resource>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(Resource entity)
    {
        SpecificationHelper.ValidateGuid(entity.SalonId, _result, "SalonId is required (Cannot be Guid.Empty).");
        SpecificationHelper.ValidateGuid(entity.CreatedByEmployeeId, _result, "EmployeeId is required (Cannot be Guid.Empty).");
        SpecificationHelper.ValidateString(entity.Name, _result, "Resource name is required.");
        SpecificationHelper.ValidateString(entity.Code, _result, "Resource code is required.");
        SpecificationHelper.ValidateOptionalStringLength(entity.Description, 500,
            _result, "Resource description cannot be longer than 500 characters.");
        SpecificationHelper.ValidateQuantity(entity.Quantity, _result, "Quantity cannot be negative");
        SpecificationHelper.ValidateOptionalUrl(entity.ImgUrl, _result, "Image URL is not valid.");

        return _result;
    }
}