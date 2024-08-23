namespace SpaManagementSystem.Application.Requests.Auth;

public record RefreshRequest(string AccessToken, string RefreshToken);