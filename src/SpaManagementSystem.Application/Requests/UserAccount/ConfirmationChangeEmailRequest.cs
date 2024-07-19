namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to confirm a change of email address.
    /// </summary>
    public class ConfirmationChangeEmailRequest
    {
        /// <summary>
        /// Gets the new email address that the user wants to set.
        /// </summary>
        public string NewEmail { get; init; }

        /// <summary>
        /// Gets the confirmation token required to validate the email change request.
        /// </summary>
        public string Token { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationChangeEmailRequest"/> class.
        /// </summary>
        /// <param name="newEmail">The new email address that the user wants to set.</param>
        /// <param name="token">The confirmation token required to validate the email change request.</param>
        public ConfirmationChangeEmailRequest(string newEmail, string token)
        {
            NewEmail = newEmail;
            Token = token;
        }
    }
}