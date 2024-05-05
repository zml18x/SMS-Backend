using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for user-related services within the Spa Management System.
    /// This interface manages tasks such as creating profiles, retrieving account details, and updating user profiles.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Asynchronously creates a new user profile with the specified details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="gender">The gender of the user.</param>
        /// <param name="dateOfBirth">The date of birth of the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task CreateProfileAsync(Guid userId, string firstName, string lastName, string gender,
            DateOnly dateOfBirth);

        /// <summary>
        /// Asynchronously retrieves detailed account information for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="phoneNumber">The phone number of the user.</param>
        /// <returns>A task representing the asynchronous operation, containing the user account details.</returns>
        public Task<UserAccountDetailsDto> GetAccountDetailsAsync(Guid userId, string email, string phoneNumber);

        /// <summary>
        /// Asynchronously updates a user's profile with the provided details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="firstName">Optional. The new first name of the user; if null, the first name is not updated.</param>
        /// <param name="lastName">Optional. The new last name of the user; if null, the last name is not updated.</param>
        /// <param name="gender">Optional. The new gender of the user; if null, the gender is not updated.</param>
        /// <param name="dateOfBirth">Optional. The new date of birth of the user; if null, the date of birth is not updated.</param>
        /// <returns>A task representing the asynchronous operation, resulting in a boolean
        /// indicating if the update was successful.</returns>
        public Task<bool> UpdateProfileAsync(Guid userId, string? firstName, string? lastName, string? gender,
            DateOnly? dateOfBirth);
    }
}