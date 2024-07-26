namespace SpaManagementSystem.Application.Requests.UserAccount;

/// <summary>
/// Represents a request for user sign-in.
/// </summary>
public record SignInRequest(string Email, string Password);