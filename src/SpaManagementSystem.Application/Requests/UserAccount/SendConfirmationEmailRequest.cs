namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request for sending a confirmation email to a user.
    /// This class is used to capture the necessary data, specifically the user's email address,
    /// to send an email for confirming account creation or changes.
    /// </summary>
    public class SendConfirmationEmailRequest
    {
        /// <summary>
        /// Gets the email address to which the confirmation email will be sent.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendConfirmationEmailRequest"/> class with a specified email address.
        /// </summary>
        /// <param name="email">The email address of the user who will receive the confirmation email.</param>
        public SendConfirmationEmailRequest(string email)
        {
            Email = email;
        }
    }
}