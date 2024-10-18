using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Builders;

public class AddressBuilder(ISpecification<Address> specification) : IBuilder<Address>
{
#nullable disable
    private string _country;
    private string _region;
    private string _city;
    private string _postalCode;
    private string _street;
    private string _buildingNumber;
#nullable enable
    
    
    
    public Address Build()
    {
        var address = new Address(_country, _region, _city, _postalCode, _street, _buildingNumber);

        var validationResult = specification.IsSatisfiedBy(address);
        if (!validationResult.IsValid)
            throw new DomainValidationException($"Address creation failed: {string.Join(", ", validationResult.Errors)}");

        return address;
    }

    public AddressBuilder WithCountry(string country)
    {
        _country = country;
        return this;
    }
    
    public AddressBuilder WithRegion(string region)
    {
        _region = region;
        return this;
    }
    
    public AddressBuilder WithCity(string city)
    {
        _city = city;
        return this;
    }
    
    public AddressBuilder WithPostalCode(string postalCode)
    {
        _postalCode = postalCode;
        return this;
    }
    
    public AddressBuilder WithStreet(string street)
    {
        _street = street;
        return this;
    }
    
    public AddressBuilder WithBuildingNumber(string buildingNumber)
    {
        _buildingNumber = buildingNumber;
        return this;
    }
}