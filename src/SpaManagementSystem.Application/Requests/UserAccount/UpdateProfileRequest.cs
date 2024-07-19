namespace SpaManagementSystem.Application.Requests.UserAccount
{
    /// <summary>
    /// Represents a request to update a user's profile information.
    /// </summary>
    public class UpdateProfileRequest
    {
        /// <summary>
        /// Gets or sets the first name of the user. This field is optional.
        /// </summary>
        public string? FirstName { get; init; }

        /// <summary>
        /// Gets or sets the last name of the user. This field is optional.
        /// </summary>
        public string? LastName { get; init; }

        /// <summary>
        /// Gets or sets the gender of the user. This field is optional.
        /// </summary>
        public string? Gender { get; init; }

        /// <summary>
        /// Gets or sets the date of birth of the user. This field is optional.
        /// </summary>
        public DateOnly? DateOfBirth { get; init; }
    }
}