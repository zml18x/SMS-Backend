namespace SpaManagementSystem.Application.Requests.Auth;

/// <summary>
/// Represents a request to send a reset password token.
/// </summary>
public record SendResetPasswordTokenRequest(string Email, string NewPassword);