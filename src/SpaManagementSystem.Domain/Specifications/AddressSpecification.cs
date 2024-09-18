using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Specifications;

public class AddressSpecification : ISpecification<Address>
{
    public ValidationResult IsSatisfiedBy(Address entity)
    {
        var result = new ValidationResult(true);
        
        ValidateCountry(entity.Country, result);
        ValidateRegion(entity.Region,result);
        ValidateCity(entity.City,result);
        ValidatePostalCode(entity.PostalCode,result);
        ValidateStreet(entity.Street, result);
        ValidateBuildingNumber(entity.BuildingNumber, result);
        
        return result;
    }

    private void ValidateCountry(string country, ValidationResult result)
    {
        if(string.IsNullOrWhiteSpace(country))
            result.AddError("Country is required");
    }
    
    private void ValidateRegion(string region, ValidationResult result)
    {
        if(string.IsNullOrWhiteSpace(region))
            result.AddError("Region is required");
    }
    
    private void ValidateCity(string city, ValidationResult result)
    {
        if(string.IsNullOrWhiteSpace(city))
            result.AddError("City is required");
    }
    
    private void ValidatePostalCode(string postalCode, ValidationResult result)
    {
        if(string.IsNullOrWhiteSpace(postalCode))
            result.AddError("Postal code is required");
    }
    
    private void ValidateStreet(string street, ValidationResult result)
    {
        if(string.IsNullOrWhiteSpace(street))
            result.AddError("Street is required");
    }
    
    private void ValidateBuildingNumber(string buildingNumber, ValidationResult result)
    {
        if(string.IsNullOrWhiteSpace(buildingNumber))
            result.AddError("Building number is required");
    }
}