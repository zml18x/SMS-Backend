using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.UserAccount;


namespace SpaManagementSystem.Application.Services;

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
    /// <exception cref="NotFoundException">Thrown when the user profile is not found for the specified user id.</exception>
    public async Task<UserProfileDto> GetProfileAsync(Guid userId)
    {
        var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

        if (userProfile == null)
            throw new NotFoundException($"No profile found for user ID '{userId}'.");

        return new UserProfileDto(userProfile.FirstName, userProfile.LastName,
            userProfile.Gender, userProfile.DateOfBirth);
    }

    /// <inheritdoc />
    /// <exception cref="NotFoundException">Thrown when the user profile is not found for the specified user id.</exception>
    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

        if (userProfile == null)
            throw new NotFoundException($"The user profile for user with id {userId} was not found.");

        var convertedGender = GenderTypeHelper.ConvertToGenderType(request.Gender);

        var isUpdated = userProfile.UpdateProfile(request.FirstName, request.LastName, convertedGender,
            request.DateOfBirth);

        if (isUpdated)
        {
            _userProfileRepository.Update(userProfile);
            await _userProfileRepository.SaveChangesAsync();
        }

        return isUpdated;
    }
}