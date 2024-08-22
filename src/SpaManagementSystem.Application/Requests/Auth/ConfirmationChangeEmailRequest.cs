namespace SpaManagementSystem.Application.Requests.Auth;

/// <summary>
/// Represents a request to confirm a change of email address.
/// </summary>
public record ConfirmationChangeEmailRequest(string NewEmail, string Token);