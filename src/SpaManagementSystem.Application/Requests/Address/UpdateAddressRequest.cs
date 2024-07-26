namespace SpaManagementSystem.Application.Requests.Address;

/// <summary>
/// Represents a request for updating an existing address.
/// </summary>
public record UpdateAddressRequest(string Country, string Region, string City, string PostalCode, string Street,
    string BuildingNumber);