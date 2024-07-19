namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to confirm an email address.
    /// </summary>
    public class ConfirmEmailRequest
    {
        /// <summary>
        /// Gets the email address to be confirmed.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the confirmation token required to validate the email confirmation request.
        /// </summary>
        public string Token { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmEmailRequest"/> class.
        /// </summary>
        /// <param name="email">The email address to be confirmed.</param>
        /// <param name="token">The confirmation token required to validate the email confirmation request.</param>
        public ConfirmEmailRequest(string email, string token)
        {
            Email = email;
            Token = token;
        }
    }
}