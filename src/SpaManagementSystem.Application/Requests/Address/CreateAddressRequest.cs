namespace SpaManagementSystem.Application.Requests.Address;

/// <summary>
/// Represents a request for creating a new address.
/// </summary>
public class CreateAddressRequest
{
    /// <summary>
    /// Gets the country of the address.
    /// </summary>
    public string Country { get; init; }

    /// <summary>
    /// Gets the region (e.g., state or province) of the address.
    /// </summary>
    public string Region { get; init; }

    /// <summary>
    /// Gets the city of the address.
    /// </summary>
    public string City { get; init; }

    /// <summary>
    /// Gets the postal code of the address.
    /// </summary>
    public string PostalCode { get; init; }

    /// <summary>
    /// Gets the street name of the address.
    /// </summary>
    public string Street { get; init; }

    /// <summary>
    /// Gets the building number of the address.
    /// </summary>
    public string BuildingNumber { get; init; }

    
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAddressRequest"/> class with the specified address details.
    /// </summary>
    /// <param name="country">The country of the address.</param>
    /// <param name="region">The region (e.g., state or province) of the address.</param>
    /// <param name="city">The city of the address.</param>
    /// <param name="postalCode">The postal code of the address.</param>
    /// <param name="street">The street name of the address.</param>
    /// <param name="buildingNumber">The building number of the address.</param>
    public CreateAddressRequest(string country, string region, string city, string postalCode, string street,
        string buildingNumber)
    {
        Country = country;
        Region = region;
        City = city;
        PostalCode = postalCode;
        Street = street;
        BuildingNumber = buildingNumber;
    }
}