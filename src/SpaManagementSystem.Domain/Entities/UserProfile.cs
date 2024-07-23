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


        
        /// <summary>
        /// Initializes a new instance of the UserProfile class.
        /// </summary>
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
        /// Updates the profile information of the entity.
        /// If any of the provided values differ from the current values,
        /// the corresponding properties are updated, and the UpdatedAt timestamp is set to the current UTC time.
        /// </summary>
        /// <param name="firstName">The new first name to update.</param>
        /// <param name="lastName">The new last name to update.</param>
        /// <param name="gender">The new gender to update.</param>
        /// <param name="dateOfBirth">The new date of birth to update.</param>
        /// <returns>Returns true if any profile information was updated, otherwise false.</returns>
        public bool UpdateProfile(string firstName, string lastName, GenderType gender, DateOnly dateOfBirth)
        {
            var anyDataUpdated = false;

            if (FirstName != firstName)
            {
                SetFirstName(firstName);
                anyDataUpdated = true;
            }

            if (LastName != lastName)
            {
                SetLastName(lastName);
                anyDataUpdated = true;
            }

            if (Gender != gender)
            {
                SetGender(gender);
                anyDataUpdated = true;
            }

            if (DateOfBirth != dateOfBirth)
            {
                SetDateOfBirth(dateOfBirth);
                anyDataUpdated = true;
            }

            if (anyDataUpdated)
                UpdatedAt = DateTime.UtcNow;

            return anyDataUpdated;
        }

        /// <summary>
        /// Validates and sets the UserId.
        /// </summary>
        /// <param name="userId">The unique identifier to set.</param>
        /// <exception cref="ArgumentException">Thrown when the UserId is empty.</exception>
        private void SetUserId(Guid userId)
        {
            UserId = (userId != Guid.Empty)
                ? userId
                : throw new ArgumentException("User id cannot be empty", nameof(userId));
        }

        /// <summary>
        /// Validates and sets the user's first name.
        /// </summary>
        /// <param name="firstName">The first name to set.</param>
        /// <exception cref="ArgumentException">Thrown when the first name is null or whitespace.</exception>
        private void SetFirstName(string firstName)
        {
            FirstName = !string.IsNullOrWhiteSpace(firstName)
                ? firstName
                : throw new ArgumentException("First name cannot be null or whitespace", nameof(firstName));
        }

        /// <summary>
        /// Validates and sets the user's last name.
        /// </summary>
        /// <param name="lastName">The last name to set.</param>
        /// <exception cref="ArgumentException">Thrown when the last name is null or whitespace.</exception>
        private void SetLastName(string lastName)
        {
            LastName = !string.IsNullOrWhiteSpace(lastName)
                ? lastName
                : throw new ArgumentException("Last name cannot be null or whitespace", nameof(lastName));
        }

        /// <summary>
        /// Validates and sets the Gender property.
        /// </summary>
        /// <param name="gender">The GenderType to set.</param>
        /// <exception cref="ArgumentException">Thrown when the value is not a defined enum value.</exception>
        private void SetGender(GenderType gender)
        {
            Gender = (Enum.IsDefined(typeof(GenderType), gender))
                ? gender
                : throw new ArgumentException("Invalid value for the GenderType.",nameof(gender));
        }
        
        /// <summary>
        /// Validates and sets the DateOfBirth.
        /// </summary>
        /// <param name="dateOfBirth">The DateOnly to set as the date of birth.</param>
        /// <exception cref="ArgumentException">Thrown when the date is in the future.</exception>
        private void SetDateOfBirth(DateOnly dateOfBirth)
        {
            DateOfBirth = (dateOfBirth <= DateOnly.FromDateTime(DateTime.UtcNow.Date))
                ? dateOfBirth
                : throw new ArgumentException("Date of birth cannot be in the future", nameof(dateOfBirth));
        }
    }
}