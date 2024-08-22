namespace SpaManagementSystem.Application.Requests.Auth;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
public record UserRegisterRequest(string Email, string Password, string PhoneNumber);