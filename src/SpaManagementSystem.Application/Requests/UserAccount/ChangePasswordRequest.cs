namespace SpaManagementSystem.Application.Requests.UserAccount;

/// <summary>
/// Represents a request to change the user's password.
/// </summary>
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);