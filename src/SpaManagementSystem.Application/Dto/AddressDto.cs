namespace SpaManagementSystem.Application.Dto;

/// <summary>
/// Data Transfer Object (DTO) for representing an address.
/// </summary>
public record AddressDto(string Country, string Region, string City, string PostalCode, string Street, string BuildingNumber);