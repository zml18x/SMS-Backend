 namespace SpaManagementSystem.Application.Requests.UserAccount;

 /// <summary>
 /// Represents a request to send a confirmation email.
 /// </summary>
 public record SendConfirmationEmailRequest(string Email);