namespace SpaManagementSystem.Application.Requests.UserAccount;

/// <summary>
/// Represents a request to change the email address.
/// </summary>
public record ChangeEmailRequest(string NewEmail);