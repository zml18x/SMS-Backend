namespace SpaManagementSystem.Application.Requests.Auth;

public record ConfirmationChangeEmailRequest(string NewEmail, string Token);