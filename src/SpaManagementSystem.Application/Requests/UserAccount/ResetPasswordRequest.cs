namespace SpaManagementSystem.Application.Requests.UserAccount;

/// <summary>
/// Represents a request to reset a user's password.
/// </summary>
public record ResetPasswordRequest(string Email, string NewPassword, string Token);