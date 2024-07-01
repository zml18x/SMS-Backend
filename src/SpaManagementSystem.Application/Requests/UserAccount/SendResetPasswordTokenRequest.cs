namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to send a reset password token to a user's email.
    /// This class is used to capture and transfer the necessary data for resetting a user's password,
    /// including their email address and the new password they wish to set.
    /// </summary>
    public class SendResetPasswordTokenRequest
    {
        /// <summary>
        /// Gets the email address of the user requesting a password reset.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the new password that the user wants to set once their identity is verified.
        /// </summary>
        public string NewPassword { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendResetPasswordTokenRequest"/> class with specified email and new password.
        /// </summary>
        /// <param name="email">The email address of the user who is requesting the password reset.</param>
        /// <param name="newPassword">The new password the user intends to set after resetting their current password.</param>
        public SendResetPasswordTokenRequest(string email, string newPassword)
        {
            Email = email;
            NewPassword = newPassword;
        }
    }
}