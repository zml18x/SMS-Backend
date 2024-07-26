namespace SpaManagementSystem.Application.Requests.UserAccount;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
public record RegisterRequest(string Email, string Password, string PhoneNumber, string FirstName,
    string LastName, string Gender, DateOnly DateOfBirth);