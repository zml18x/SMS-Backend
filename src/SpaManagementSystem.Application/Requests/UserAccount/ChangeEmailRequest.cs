namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to change a user's email address.
    /// This class is used to capture and transfer the new email address that a user wishes to update to.
    /// </summary>
    public class ChangeEmailRequest
    {
        /// <summary>
        /// Gets the new email address that the user wants to update to.
        /// </summary>
        public string NewEmail { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEmailRequest"/> class with a specific new email address.
        /// </summary>
        /// <param name="newEmail">The new email address to be updated to the user's account.</param>
        public ChangeEmailRequest(string newEmail)
        {
            NewEmail = newEmail;
        }
    }
}