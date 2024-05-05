namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request for confirming a user's email address with a verification token.
    /// This class is used to capture and transfer the necessary data for email confirmation processes.
    /// </summary>
    public class ConfirmEmailRequest
    {
        /// <summary>
        /// Gets the email address to be confirmed.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the verification token associated with the email confirmation.
        /// </summary>
        public string Token { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmEmailRequest"/> class with specified email and token.
        /// </summary>
        /// <param name="email">The email address of the user whose email needs to be confirmed.</param>
        /// <param name="token">The confirmation token that validates the email address.</param>
        public ConfirmEmailRequest(string email, string token)
        {
            Email = email;
            Token = token;
        }
    }
}