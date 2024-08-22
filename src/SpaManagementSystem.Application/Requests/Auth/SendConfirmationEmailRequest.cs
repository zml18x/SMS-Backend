 namespace SpaManagementSystem.Application.Requests.Auth;

 /// <summary>
 /// Represents a request to send a confirmation email.
 /// </summary>
 public record SendConfirmationEmailRequest(string Email);