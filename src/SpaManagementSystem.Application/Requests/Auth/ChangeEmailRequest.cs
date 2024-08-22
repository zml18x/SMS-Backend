namespace SpaManagementSystem.Application.Requests.Auth;

/// <summary>
/// Represents a request to change the email address.
/// </summary>
public record ChangeEmailRequest(string NewEmail);