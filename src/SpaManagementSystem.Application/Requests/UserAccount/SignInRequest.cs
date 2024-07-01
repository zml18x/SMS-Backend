namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request for signing in a user with an email and password.
    /// This class is used to capture and transfer user credentials for authentication purposes.
    /// </summary>
    public class SignInRequest
    {
        /// <summary>
        /// Gets the email address used for signing in.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the password associated with the email for signing in.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignInRequest"/> class with specified email and password.
        /// </summary>
        /// <param name="email">The email address of the user attempting to sign in.</param>
        /// <param name="password">The password of the user attempting to sign in.</param>
        public SignInRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}