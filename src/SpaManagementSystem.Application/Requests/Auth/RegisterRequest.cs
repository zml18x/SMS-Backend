namespace SpaManagementSystem.Application.Requests.Auth;

public record UserRegisterRequest(string Email, string Password, string PhoneNumber);