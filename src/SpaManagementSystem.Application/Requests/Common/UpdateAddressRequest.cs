namespace SpaManagementSystem.Application.Requests.Common;

/// <summary>
/// Represents a request for updating address
/// </summary>
public record UpdateAddressRequest(string Country, string Region, string City, string PostalCode, string Street,
    string BuildingNumber);