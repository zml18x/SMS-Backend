namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to update a user's profile information.
    /// This class is used to capture and transfer user profile data changes,
    /// including personal information such as name, gender, and date of birth.
    /// </summary>
    public class UpdateProfileRequest
    {
        /// <summary>
        /// Gets or sets the first name of the user. Optional; can be null if not updating.
        /// </summary>
        public string? FirstName { get; init; }

        /// <summary>
        /// Gets or sets the last name of the user. Optional; can be null if not updating.
        /// </summary>
        public string? LastName { get; init; }

        /// <summary>
        /// Gets or sets the gender of the user. Optional; can be null if not updating.
        /// This should be a valid gender identifier as defined by the application.
        /// </summary>
        public string? Gender { get; init; }

        /// <summary>
        /// Gets or sets the date of birth of the user. Optional; can be null if not updating.
        /// This should be provided in a format that the application can process, such as yyyy-MM-dd.
        /// </summary>
        public DateOnly? DateOfBirth { get; init; }
    }
}