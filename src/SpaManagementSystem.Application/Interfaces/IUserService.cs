using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Interfaces
{
    /// <summary>
    /// Defines the operations for managing user profiles within the application.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user profile with the provided details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the profile is being created.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="gender">The gender of the user.</param>
        /// <param name="dateOfBirth">The date of birth of the user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task CreateProfileAsync(Guid userId, string firstName, string lastName, string gender, DateOnly dateOfBirth);

        /// <summary>
        /// Retrieves the account details for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose account details are being retrieved.</param>
        /// <param name="email">The email address associated with the user.</param>
        /// <param name="phoneNumber">The phone number associated with the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user account details DTO.</returns>
        public Task<UserAccountDetailsDto> GetAccountDetailsAsync(Guid userId, string email, string phoneNumber);

        /// <summary>
        /// Updates an existing user profile with the provided details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose profile is being updated.</param>
        /// <param name="firstName">Optional. The new first name of the user.</param>
        /// <param name="lastName">Optional. The new last name of the user.</param>
        /// <param name="gender">Optional. The new gender of the user.</param>
        /// <param name="dateOfBirth">Optional. The new date of birth of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the update was successful.</returns>
        public Task<bool> UpdateProfileAsync(Guid userId, string? firstName, string? lastName, string? gender,
            DateOnly? dateOfBirth);
    }
}