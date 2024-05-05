using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Asynchronously creates a user profile with the provided details.
        /// </summary>
        /// <param name="userId">The unique identifier for the user. This ID must already be associated with an existing user account.</param>
        /// <param name="firstName">The first name of the user. Cannot be null or empty.</param>
        /// <param name="lastName">The last name of the user. Cannot be null or empty.</param>
        /// <param name="gender">The gender of the user as a string. This should correspond to a valid value defined in the GenderType enum.</param>
        /// <param name="dateOfBirth">The date of birth of the user. Must be a past date.</param>
        /// <returns>A Task representing the asynchronous operation, which, upon completion, signifies that the user profile has been created successfully.</returns>
        public Task CreateProfileAsync(Guid userId, string firstName, string lastName, string gender,
            DateOnly dateOfBirth);

        public Task<UserAccountDetailsDto> GetAccountDetailsAsync(Guid userId, string email, string phoneNumber);

        public Task<bool> UpdateProfileAsync(Guid userId, string? firstName, string? lastName, string? gender,
            DateOnly? dateOfBirth);
    }
}