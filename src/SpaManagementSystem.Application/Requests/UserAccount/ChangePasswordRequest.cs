namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to change the user's password.
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Gets the current password of the user.
        /// </summary>
        public string CurrentPassword { get; init; }

        /// <summary>
        /// Gets the new password that the user wants to set.
        /// </summary>
        public string NewPassword { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordRequest"/> class.
        /// </summary>
        /// <param name="currentPassword">The current password of the user.</param>
        /// <param name="newPassword">The new password to be set by the user.</param>
        public ChangePasswordRequest(string currentPassword, string newPassword)
        {
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}