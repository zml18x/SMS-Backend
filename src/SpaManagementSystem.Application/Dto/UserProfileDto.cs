namespace SpaManagementSystem.Application.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing detailed information about a user's account.
    /// </summary>
    public class UserAccountDetailsDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user. This property is optional.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user. This property is optional.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the gender of the user. This property is optional.
        /// </summary>
        public string? Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the user. This property is optional.
        /// </summary>
        public DateOnly? DateOfBirth { get; set; }

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountDetailsDto"/> class with the specified details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        /// <param name="firstName">The first name of the user. Can be null.</param>
        /// <param name="lastName">The last name of the user. Can be null.</param>
        /// <param name="gender">The gender of the user. Can be null.</param>
        /// <param name="dateOnly">The date of birth of the user. Can be null.</param>
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