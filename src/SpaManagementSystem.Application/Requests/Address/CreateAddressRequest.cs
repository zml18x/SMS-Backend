namespace SpaManagementSystem.Application.Requests.Address;

/// <summary>
/// Represents a request for creating a new address.
/// </summary>
public record CreateAddressRequest(string Country, string Region, string City, string PostalCode, string Street,
    string BuildingNumber);