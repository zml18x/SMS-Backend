namespace SpaManagementSystem.Application.Requests.UserAccount;

/// <summary>
/// Represents a request to update a user's profile information.
/// </summary>
public record UpdateProfileRequest(string FirstName, string LastName, string Gender, DateOnly DateOfBirth);