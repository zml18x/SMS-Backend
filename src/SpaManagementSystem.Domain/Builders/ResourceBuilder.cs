using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.Builders;

public class ResourceBuilder(ISpecification<Resource> specification) : IBuilder<Resource>
{
#nullable disable
    private Guid _id = Guid.Empty;
    private Guid _salonId;
    private Guid _createdByEmployeeId;
    private string _name;
    private string _code;
    private int _quantity;
#nullable enable
    private string? _description;
    private string? _imgUrl;
    
    
    
    public Resource Build()
    {
        var resource = new Resource(_id, _salonId, _createdByEmployeeId, _name, _code, _description, _quantity, _imgUrl);

        var validationResult = specification.IsSatisfiedBy(resource);

        if (!validationResult.IsValid)
            throw new DomainValidationException($"Resource creation failed: {string.Join(", ", validationResult.Errors)}");

        return resource;
    }
    
    public ResourceBuilder WithResourceId(Guid resourceId)
    {
        _id = resourceId;
        return this;
    }
         
    public ResourceBuilder WithSalonId(Guid salonId)
    {
        _salonId = salonId;
        return this;
    }
         
    public ResourceBuilder WithEmployeeId(Guid createdByEmployeeId)
    {
        _createdByEmployeeId = createdByEmployeeId;
        return this;
    }
     
    public ResourceBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
     
    public ResourceBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }
     
    public ResourceBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }
    
    public ResourceBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public ResourceBuilder WithImgUrl(string imgUrl)
    {
        _imgUrl = imgUrl;
        return this;
    }
}