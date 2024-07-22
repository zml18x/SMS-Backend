namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to send a reset password token.
    /// </summary>
    public class SendResetPasswordTokenRequest
    {
        /// <summary>
        /// Gets the email address associated with the account requesting a password reset.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the new password to be set once the reset token is validated.
        /// </summary>
        public string NewPassword { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SendResetPasswordTokenRequest"/> class.
        /// </summary>
        /// <param name="email">The email address associated with the account requesting a password reset.</param>
        /// <param name="newPassword">The new password to be set once the reset token is validated.</param>
        public SendResetPasswordTokenRequest(string email, string newPassword)
        {
            Email = email;
            NewPassword = newPassword;
        }
    }
}