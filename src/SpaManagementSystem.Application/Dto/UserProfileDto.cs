namespace SpaManagementSystem.Application.Dto
{
    /// <summary>
    /// Represents detailed information about a user account, used to transfer data within the application.
    /// Th cis DTO includes basic user identification andontact information along with optional personal details.
    /// </summary>
    public class UserAccountDetailsDto
    {
 /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user's first name. Optional.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name. Optional.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's gender. Optional.
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// Gets or sets the user's date of birth. Optional.
        /// </summary>
        public DateOnly? DateOfBirth { get; set; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountDetailsDto"/> class with specified user details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        /// <param name="firstName">The first name of the user (optional).</param>
        /// <param name="lastName">The last name of the user (optional).</param>
        /// <param name="gender">The gender of the user (optional).</param>
        /// <param name="dateOnly">The date of birth of the user (optional).</param>
        public UserAccountDetailsDto(Guid userId, string email, string phoneNumber, string? firstName = null,
            string? lastName = null, string? gender = null, DateOnly? dateOnly = null)
        {
            UserId = userId;
            Email = email;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOnly;
        }
    }
}