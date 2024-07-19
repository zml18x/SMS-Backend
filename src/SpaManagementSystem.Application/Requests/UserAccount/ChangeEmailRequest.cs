namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to change the email address.
    /// </summary>
    public class ChangeEmailRequest
    {
        /// <summary>
        /// Gets the new email address to be set.
        /// </summary>
        public string NewEmail { get; init; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmailRequest"/> class.
        /// </summary>
        /// <param name="newEmail">The new email address to be set.</param>
        public ChangeEmailRequest(string newEmail)
        {
            NewEmail = newEmail;
        }
    }
}