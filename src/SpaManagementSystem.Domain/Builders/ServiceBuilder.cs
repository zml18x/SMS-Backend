using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.Builders;

public class ServiceBuilder(ISpecification<Service> specification) : IBuilder<Service>
{
#nullable disable
    private Guid _id = Guid.Empty;
    private Guid _salonId;
    private string _name;
    private string _code;
    private decimal _price;
    private decimal _taxRate;
    private TimeSpan _duration;
#nullable enable
    private string? _description;
    private string? _imgUrl;
    
    
    
    public Service Build()
    {
        var service = new Service(_id, _salonId, _name, _code, _description, _price, _taxRate, _duration, _imgUrl);
        
        var validationResult = specification.IsSatisfiedBy(service);

        if (!validationResult.IsValid)
            throw new DomainValidationException($"Service creation failed: {string.Join(", ", validationResult.Errors)}");

        return service;
    }
    
    public ServiceBuilder WithServiceId(Guid serviceId)
    {
        _id = serviceId;
        return this;
    }
         
    public ServiceBuilder WithSalonId(Guid salonId)
    {
        _salonId = salonId;
        return this;
    }
     
    public ServiceBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
     
    public ServiceBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }
     
    public ServiceBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public ServiceBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ServiceBuilder WithTaxRate(decimal taxRate)
    {
        _taxRate = taxRate;
        return this;
    }

    public ServiceBuilder WithDuration(TimeSpan duration)
    {
        _duration = duration;
        return this;
    }

    public ServiceBuilder WithImgUrl(string? imgUrl)
    {
        _imgUrl = imgUrl;
        return this;
    }
}