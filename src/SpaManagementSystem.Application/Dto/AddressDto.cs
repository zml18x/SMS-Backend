namespace SpaManagementSystem.Application.Dto;

public record AddressDto(
    string Country,
    string Region,
    string City,
    string PostalCode,
    string Street,
    string BuildingNumber);