using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Application.Interfaces;


namespace SpaManagementSystem.Application.Services
{
    /// <summary>
    /// Service responsible for managing user profile data. Implements <see cref="IUserService"/>.
    /// This service handles operations such as creating, retrieving, and updating user profiles.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserProfileRepository _userProfileRepository;



        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> with a repository for handling user profile data.
        /// </summary>
        /// <param name="userProfileRepository">The repository to manage user profile persistence.</param>
        public UserService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }



        /// <summary>
        /// Asynchronously creates a new user profile associated with the specified user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the profile is being created.
        /// This ID links the profile directly to an existing user account.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="gender">The gender of the user, as a string that will be converted to the GenderType enum.</param>
        /// <param name="dateOfBirth">The date of birth of the user.</param>
        /// <exception cref="InvalidOperationException">Thrown if a user profile with the specified user ID already exists.</exception>
        /// <returns>A task that represents the asynchronous operation, with no return value.</returns>
        public async Task CreateProfileAsync(Guid userId, string firstName, string lastName, string gender,
            DateOnly dateOfBirth)
        {
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

            if (userProfile != null)
                throw new InvalidOperationException("User profile already exists for the specified user ID.");

            userProfile = new UserProfile(Guid.NewGuid(), userId, firstName, lastName,
                GenderTypeHelper.ConvertToGenderType(gender), dateOfBirth);

            await _userProfileRepository.CreateAsync(userProfile);
            await _userProfileRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves detailed account information for a specific user based on their user ID.
        /// </summary>
        /// <param name="userId">The unique identifier for the user whose details are being retrieved.</param>
        /// <param name="email">The email address to include in the account details.</param>
        /// <param name="phoneNumber">The phone number to include in the account details.</param>
        /// <returns>A task that results in a <see cref="UserAccountDetailsDto"/> containing the user's profile information.
        /// If no profile exists, the returned DTO will include only the userId, email, and phoneNumber provided.</returns>
        public async Task<UserAccountDetailsDto> GetAccountDetailsAsync(Guid userId, string email, string phoneNumber)
        {
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

            if (userProfile == null)
                return new UserAccountDetailsDto(userId, email, phoneNumber);

            return new UserAccountDetailsDto(userId, email, phoneNumber, userProfile.FirstName, userProfile.LastName,
                userProfile.Gender.ToString(), userProfile.DateOfBirth);
        }

        /// <summary>
        /// Asynchronously updates a user's profile information with the provided details.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose profile is to be updated.</param>
        /// <param name="firstName">Optional. The new first name to update the profile with. If null, the first name is not updated.</param>
        /// <param name="lastName">Optional. The new last name to update the profile with. If null, the last name is not updated.</param>
        /// <param name="gender">Optional. The new gender to update the profile with. If null, the gender is not updated.</param>
        /// <param name="dateOfBirth">Optional. The new date of birth to update the profile with. If null, the date of birth is not updated.</param>
        /// <returns>A task resulting in a boolean indicating if any profile details were successfully updated.</returns>
        /// <exception cref="NotFoundException">Thrown if no user profile is found for the given userId,
        /// indicating that the profile cannot be updated because it does not exist.</exception>
        public async Task<bool> UpdateProfileAsync(Guid userId, string? firstName, string? lastName, string? gender,
            DateOnly? dateOfBirth)
        {
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

            if (userProfile == null)
                throw new NotFoundException($"The user profile for user with id {userId} was not found.");
            
            GenderType? convertedGender = gender == null ? null : GenderTypeHelper.ConvertToGenderType(gender);
            
            var isUpdated = userProfile.UpdateProfile(firstName, lastName, convertedGender, dateOfBirth);

            if (isUpdated)
            {
                _userProfileRepository.Update(userProfile);
                await _userProfileRepository.SaveChangesAsync();
            }

            return isUpdated;
        }
    }
}