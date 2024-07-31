namespace SpaManagementSystem.Domain.ValueObjects;

/// <summary>
/// Represents an address in the spa management system.
/// </summary>
public record Address(string Country, string Region, string City, string PostalCode, string Street, string BuildingNumber);