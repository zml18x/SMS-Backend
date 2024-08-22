namespace SpaManagementSystem.Application.Requests.Auth;

/// <summary>
/// Represents a request for user sign-in.
/// </summary>
public record SignInRequest(string Email, string Password);