using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class ServiceSpecification : ISpecification<Service>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(Service entity)
    {
        SpecificationHelper.ValidateGuid(entity.SalonId, _result, "SalonId is required (Cannot be Guid.Empty).");
        SpecificationHelper.ValidateString(entity.Name, _result, "Service name is required.");
        SpecificationHelper.ValidateString(entity.Code, _result, "Service code is required.");
        SpecificationHelper.ValidateOptionalStringLength(entity.Description, 1000,
            _result, "Service description cannot be longer than 1000 characters.");
        SpecificationHelper.ValidatePrice(entity.Price, _result, "Price cannot be negative.");
        SpecificationHelper.ValidateTaxRate(entity.TaxRate, _result, "Tax rate must be between 0 and 1.");
        ValidateDuration(entity.Duration);
        SpecificationHelper.ValidateOptionalUrl(entity.ImgUrl, _result, "Image URL is not valid.");
        
        return _result;
    }
    
    private void ValidateDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            _result.AddError("Duration must be greater than zero.");
        
        if (duration.TotalHours > 8)
            _result.AddError("Duration cannot exceed 8 hours.");
    }
}