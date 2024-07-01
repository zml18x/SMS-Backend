namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request for changing a user's password.
    /// This class is used to capture and transfer the necessary data for updating a user's password,
    /// including the current password for verification and the new password to be set.
    /// </summary>
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Gets the current password of the user. This password will be verified before the new password is applied.
        /// </summary>
        public string CurrentPassword { get; init; }

        /// <summary>
        /// Gets the new password that the user wishes to set. This must meet the system's security standards.
        /// </summary>
        public string NewPassword { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordRequest"/> class with the user's current and new passwords.
        /// </summary>
        /// <param name="currentPassword">The user's current password, used for verification purposes.</param>
        /// <param name="newPassword">The user's desired new password, which will replace the current one upon successful verification.</param>
        public ChangePasswordRequest(string currentPassword, string newPassword)
        {
            CurrentPassword = currentPassword;
            NewPassword = newPassword;
        }
    }
}