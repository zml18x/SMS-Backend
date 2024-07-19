namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to register a new user.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets the email address of the user to register.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// Gets the password for the new user account.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// Gets the phone number of the user.
        /// </summary>
        public string PhoneNumber { get; init; }

        /// <summary>
        /// Gets the first name of the user.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Gets the last name of the user.
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// Gets the gender of the user.
        /// </summary>
        public string Gender { get; init; }

        /// <summary>
        /// Gets the date of birth of the user.
        /// </summary>
        public DateOnly DateOfBirth { get; init; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterRequest"/> class.
        /// </summary>
        /// <param name="email">The email address of the user to register.</param>
        /// <param name="password">The password for the new user account.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="gender">The gender of the user.</param>
        /// <param name="dateOfBirth">The date of birth of the user.</param>
        public RegisterRequest(string email, string password, string phoneNumber, string firstName, string lastName,
            string gender, DateOnly dateOfBirth)
        {
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
        }
    }
}