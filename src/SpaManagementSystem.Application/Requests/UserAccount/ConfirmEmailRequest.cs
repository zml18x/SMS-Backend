namespace SpaManagementSystem.Application.Requests.UserAccount;

/// <summary>
/// Represents a request to confirm an email address.
/// </summary>
public record ConfirmEmailRequest(string Email, string Token);