using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities
{
    /// <summary>
    /// Represents the user profile information within the Spa Management System.
    /// Inherits from BaseEntity and includes additional fields specific to a user's profile such as name, gender, and date of birth.
    /// </summary>
    public class UserProfile : BaseEntity
    {
        /// <summary>
        /// Gets the unique identifier for the associated user.
        /// </summary>
        public Guid UserId { get; protected set; }

        /// <summary>
        /// Gets the first name of the user.
        /// </summary>
        public string FirstName { get; protected set; } = String.Empty;

        /// <summary>
        /// Gets the last name of the user.
        /// </summary>
        public string LastName { get; protected set; } = String.Empty;

        /// <summary>
        /// Gets the gender of the user, as defined in the GenderType enum.
        /// </summary>
        public GenderType Gender { get; protected set; }

        /// <summary>
        /// Gets the date of birth of the user.
        /// </summary>
        public DateOnly DateOfBirth { get; protected set; }



        public UserProfile(){}
        
        /// <summary>
        /// Initializes a new instance of the UserProfile with specific details.
        /// </summary>
        /// <param name="id">The unique identifier for the profile.</param>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="gender">The user's gender as defined by the GenderType enum.</param>
        /// <param name="dateOfBirth">The user's date of birth.</param>
        public UserProfile(Guid id, Guid userId, string firstName, string lastName, GenderType gender,
            DateOnly dateOfBirth) : base(id)
        {
            SetUserId(userId);
            SetFirstName(firstName);
            SetLastName(lastName);
            SetGender(gender);
            SetDateOfBirth(dateOfBirth);
        }



        /// <summary>
        /// Updates the profile with optional new values for first name, last name, gender, and date of birth.
        /// Any provided values will replace the current ones. If a field is not provided, it will not be updated.
        /// </summary>
        /// <param name="firstName">Optional. The new first name to update. If null or whitespace, the first name remains unchanged.</param>
        /// <param name="lastName">Optional. The new last name to update. If null or whitespace, the last name remains unchanged.</param>
        /// <param name="gender">Optional. The new gender to update. If null, the gender remains unchanged.</param>
        /// <param name="dateOfBirth">Optional. The new date of birth to update. If null, the date of birth remains unchanged.</param>
        /// <returns>True if any data was updated; otherwise, false.
        /// This can be used to determine if the entity needs to be saved to the database.</returns>
        public bool UpdateProfile(string? firstName = null, string? lastName = null, GenderType? gender = null,
            DateOnly? dateOfBirth = null)
        {
            var anyDataUpdated = false;

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                SetFirstName(firstName);
                anyDataUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                SetLastName(lastName);
                anyDataUpdated = true;
            }

            if (gender != null)
            {
                SetGender((GenderType)gender);
                anyDataUpdated = true;
            }

            if (dateOfBirth != null)
            {
                SetDateOfBirth((DateOnly)dateOfBirth);
                anyDataUpdated = true;
            }

            if (anyDataUpdated)
                UpdatedAt = DateTime.UtcNow;

            return anyDataUpdated;
        }

        /// <summary>
        /// Validates and sets the UserId. Throws an ArgumentException if the UserId is empty.
        /// </summary>
        /// <param name="userId">The unique identifier to set.</param>
        private void SetUserId(Guid userId)
        {
            UserId = (userId != Guid.Empty)
                ? userId
                : throw new ArgumentException("User id cannot be empty", nameof(userId));
        }

        /// <summary>
        /// Validates and sets the user's first name. Throws an ArgumentException if the first name are null or whitespace.
        /// </summary>
        /// <param name="firstName">The first name to set.</param>
        private void SetFirstName(string firstName)
        {
            FirstName = !string.IsNullOrWhiteSpace(firstName)
                ? firstName
                : throw new ArgumentException("First name cannot be null or whitespace", nameof(firstName));
        }

        /// <summary>
        /// Validates and sets the user's last name. Throws an ArgumentException if the last name are null or whitespace.
        /// </summary>
        /// <param name="lastName">The last name to set.</param>
        private void SetLastName(string lastName)
        {
            LastName = !string.IsNullOrWhiteSpace(lastName)
                ? lastName
                : throw new ArgumentException("Last name cannot be null or whitespace", nameof(lastName));
        }

        /// <summary>
        /// Validates and sets the Gender property. Throws an ArgumentException if the value is not a defined enum value.
        /// </summary>
        /// <param name="gender">The GenderType to set.</param>
        private void SetGender(GenderType gender)
        {
            Gender = (Enum.IsDefined(typeof(GenderType), gender))
                ? gender
                : throw new ArgumentException("Invalid value for the GenderType enum.",nameof(gender));
        }
        
        /// <summary>
        /// Validates and sets the DateOfBirth. Throws an ArgumentException if the date is in the future.
        /// </summary>
        /// <param name="dateOfBirth">The DateOnly to set as the date of birth.</param>
        private void SetDateOfBirth(DateOnly dateOfBirth)
        {
            DateOfBirth = (dateOfBirth <= DateOnly.FromDateTime(DateTime.UtcNow.Date))
                ? dateOfBirth
                : throw new ArgumentException("Date of birth cannot be in the future", nameof(dateOfBirth));
        }
    }
}