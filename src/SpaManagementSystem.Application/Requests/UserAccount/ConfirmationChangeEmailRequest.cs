namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to confirm the change of a user's email address using a verification token.
    /// This class is used to capture and transfer the necessary data for confirming the update of an email address.
    /// </summary>
    public class ConfirmationChangeEmailRequest
    {
        /// <summary>
        /// Gets the new email address that the user wants to update to.
        /// </summary>
        public string NewEmail { get; init; }

        /// <summary>
        /// Gets the token used to verify the email change request.
        /// </summary>
        public string Token { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationChangeEmailRequest"/> class with specified new email and token.
        /// </summary>
        /// <param name="newEmail">The new email address to be confirmed.</param>
        /// <param name="token">The verification token associated with the email change request.</param>
        public ConfirmationChangeEmailRequest(string newEmail, string token)
        {
            NewEmail = newEmail;
            Token = token;
        }
    }
}