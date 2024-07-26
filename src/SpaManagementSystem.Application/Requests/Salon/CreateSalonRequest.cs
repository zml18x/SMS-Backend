namespace SpaManagementSystem.Application.Requests.Salon;

/// <summary>
/// Represents a request to create a new salon with the specified details.
/// </summary>
public record CreateSalonRequest(string Name, string PhoneNumber, string Email);