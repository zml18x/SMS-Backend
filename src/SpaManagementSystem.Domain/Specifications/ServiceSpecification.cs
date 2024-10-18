using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class ServiceSpecification : ISpecification<Service>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(Service entity)
    {
        ValidateSalonId(entity.SalonId);
        ValidateCreatedByEmployeeId(entity.CreatedByEmployeeId);
        ValidateName(entity.Name);
        ValidateCode(entity.Code);
        ValidateDescription(entity.Description);
        ValidatePrice(entity.Price);
        ValidateTaxRate(entity.TaxRate);
        ValidateDuration(entity.Duration);
        ValidateImgUrl(entity.ImgUrl);
        
        return _result;
    }
    
    private void ValidateSalonId(Guid salonId)
    {
        if (salonId == Guid.Empty)
            _result.AddError("SalonId is required (Cannot be Guid.Empty).");
    }
    
    private void ValidateCreatedByEmployeeId(Guid createdByEmployeeId)
    {
        if (createdByEmployeeId == Guid.Empty)
            _result.AddError("EmployeeId is required (Cannot be Guid.Empty).");
    }
    
    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            _result.AddError("Name is required.");
    }
    
    private void ValidateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            _result.AddError("Code is required.");
    }
    
    private void ValidateDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description) && description.Length > 1000)
            _result.AddError("Description cannot be longer than 1000 characters.");
    }
    
    private void ValidatePrice(decimal price)
    {
        if (price < 0)
            _result.AddError("Price cannot be negative.");
    }

    private void ValidateTaxRate(decimal saleTaxRate)
    {
        if (saleTaxRate < 0 || saleTaxRate > 1)
            _result.AddError("Tax rate must be between 0 and 1.");
    }
    
    private void ValidateDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            _result.AddError("Duration must be greater than zero.");
        
        if (duration.TotalHours > 8)
            _result.AddError("Duration cannot exceed 8 hours.");
    }
    
    private void ValidateImgUrl(string? imgUrl)
    {
        if (!string.IsNullOrWhiteSpace(imgUrl) && !Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute))
            _result.AddError("Image URL is not valid.");
    }
}