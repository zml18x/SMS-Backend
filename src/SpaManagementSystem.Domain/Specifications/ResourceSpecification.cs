using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class ResourceSpecification : ISpecification<Resource>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(Resource entity)
    {
        ValidateSalonId(entity.SalonId);
        ValidateCreatedByEmployeeId(entity.CreatedByEmployeeId);
        ValidateName(entity.Name);
        ValidateCode(entity.Code);
        ValidateDescription(entity.Description);
        ValidateQuantity(entity.Quantity);
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
            _result.AddError("Resource name is required.");
    }
    
    private void ValidateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            _result.AddError("Code is required.");
    }
    
    private void ValidateDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description) && description.Length > 500)
            _result.AddError("Resource description cannot be longer than 500 characters.");
    }
    
    private void ValidateQuantity(int stockQuantity)
    {
        if (stockQuantity < 0)
            _result.AddError("Quantity cannot be negative.");
    }
    
    private void ValidateImgUrl(string? imgUrl)
    {
        if (!string.IsNullOrWhiteSpace(imgUrl) && !Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute))
            _result.AddError("Image URL is not valid.");
    }
}