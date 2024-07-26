namespace SpaManagementSystem.Application.Requests.Salon;

/// <summary>
/// Represents a request to update the details of an existing salon.
/// </summary>
public record UpdateSalonDetailsRequest(string Name, string Email, string PhoneNumber, string? Description);