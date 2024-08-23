namespace SpaManagementSystem.Application.Requests.Auth;

public record SendResetPasswordTokenRequest(string Email, string NewPassword);