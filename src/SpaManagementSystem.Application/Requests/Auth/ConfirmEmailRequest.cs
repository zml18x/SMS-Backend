namespace SpaManagementSystem.Application.Requests.Auth;

/// <summary>
/// Represents a request to confirm an email address.
/// </summary>
public record ConfirmEmailRequest(string Email, string Token);