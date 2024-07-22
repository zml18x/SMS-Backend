namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to reset a user's password.
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// Gets the email address associated with the account for which the password is being reset.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the new password to set for the user.
        /// </summary>
        public string NewPassword { get; init; }

        /// <summary>
        /// Gets the token used to confirm the password reset request.
        /// </summary>
        public string Token { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordRequest"/> class.
        /// </summary>
        /// <param name="email">The email address associated with the account.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <param name="token">The token used to confirm the password reset request.</param>
        public ResetPasswordRequest(string email, string newPassword, string token)
        {
            Email = email;
            NewPassword = newPassword;
            Token = token;
        }
    }
}