namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request for user sign-in.
    /// </summary>
    public class SignInRequest
    {
        /// <summary>
        /// Gets the email address of the user attempting to sign in.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the password for the user attempting to sign in.
        /// </summary>
        public string Password { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SignInRequest"/> class.
        /// </summary>
        /// <param name="email">The email address of the user attempting to sign in.</param>
        /// <param name="password">The password for the user attempting to sign in.</param>
        public SignInRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}