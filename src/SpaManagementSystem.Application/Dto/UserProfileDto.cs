using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a user profile.
    /// </summary>
    public class UserProfileDto
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the gender of the user.
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the user.
        /// </summary>
        public DateOnly DateOfBirth { get; set; }
        
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileDto"/> class with the specified details.
        /// </summary>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="gender">The gender of the user.</param>
        /// <param name="dateOfBirth">The date of birth of the user.</param>
        public UserProfileDto(string firstName, string lastName, GenderType gender, DateOnly dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
        }
    }
}