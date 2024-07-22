 namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to send a confirmation email.
    /// </summary>
    public class SendConfirmationEmailRequest
    {
        /// <summary>
        /// Gets the email address to which the confirmation email will be sent.
        /// </summary>
        public string Email { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SendConfirmationEmailRequest"/> class.
        /// </summary>
        /// <param name="email">The email address to send the confirmation email to.</param>
        public SendConfirmationEmailRequest(string email)
        {
            Email = email;
        }
    }
}