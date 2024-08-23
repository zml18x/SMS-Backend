namespace SpaManagementSystem.Application.Requests.Auth;

/// <summary>
/// Represents a request to refresh jwt token.
/// </summary>
public record RefreshRequest(string AccessToken, string RefreshToken);