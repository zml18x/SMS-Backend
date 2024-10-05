namespace SpaManagementSystem.Application.Requests.Salon;

public record UpdateSalonRequest(string Name, string Email, string PhoneNumber, string? Description);