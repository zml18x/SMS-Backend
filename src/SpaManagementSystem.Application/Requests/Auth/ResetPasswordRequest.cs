namespace SpaManagementSystem.Application.Requests.Auth;

public record ResetPasswordRequest(string Email, string NewPassword, string Token);