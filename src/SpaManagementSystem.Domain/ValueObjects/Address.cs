namespace SpaManagementSystem.Domain.ValueObjects;

public record Address(string Country, string Region, string City, string PostalCode, string Street, string BuildingNumber);