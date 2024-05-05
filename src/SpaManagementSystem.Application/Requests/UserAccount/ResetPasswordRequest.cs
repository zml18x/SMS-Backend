namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request for resetting a user's password.
    /// This class is used to capture and transfer the necessary data for completing a password reset process,
    /// including the user's email, a new password, and a verification token.
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// Gets the email address of the user requesting the password reset.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the new password that the user wishes to use once their identity is verified.
        /// </summary>
        public string NewPassword { get; init; }

        /// <summary>
        /// Gets the verification token used to authorize the password reset request.
        /// </summary>
        public string Token { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordRequest"/> class with specified email, new password, and token.
        /// </summary>
        /// <param name="email">The email address of the user who is requesting the password reset.</param>
        /// <param name="newPassword">The new password the user intends to set after their identity is confirmed.</param>
        /// <param name="token">The verification token that validates the authenticity of the password reset request.</param>
        public ResetPasswordRequest(string email, string newPassword, string token)
        {
            Email = email;
            NewPassword = newPassword;
            Token = token;
        }
    }
}