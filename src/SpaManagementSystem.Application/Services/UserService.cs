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



        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown when the user profile already exist for the specified user id.</exception>
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

        /// <inheritdoc />
        public async Task<UserAccountDetailsDto> GetAccountDetailsAsync(Guid userId, string email, string phoneNumber)
        {
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

            if (userProfile == null)
                return new UserAccountDetailsDto(userId, email, phoneNumber);

            return new UserAccountDetailsDto(userId, email, phoneNumber, userProfile.FirstName, userProfile.LastName,
                userProfile.Gender.ToString(), userProfile.DateOfBirth);
        }

        /// <inheritdoc />
        /// <exception cref="NotFoundException">Thrown when the user profile is not found for the specified user id.</exception>
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