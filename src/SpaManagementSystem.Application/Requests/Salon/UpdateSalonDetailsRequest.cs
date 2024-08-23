namespace SpaManagementSystem.Application.Requests.Salon;

public record UpdateSalonDetailsRequest(string Name, string Email, string PhoneNumber, string? Description);