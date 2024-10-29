namespace SpaManagementSystem.Application.Requests.Common;

public record UpdateAddressRequest(
    string Country,
    string Region,
    string City,
    string PostalCode,
    string Street,
    string BuildingNumber);