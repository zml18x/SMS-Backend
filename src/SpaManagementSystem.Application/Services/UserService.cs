using AutoMapper;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.UserAccount;
using SpaManagementSystem.Application.Extensions.RepositoryExtensions;

namespace SpaManagementSystem.Application.Services;

/// <summary>
/// Service responsible for managing user profile. Implements <see cref="IUserService"/>.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IMapper _mapper;



    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> with a repository for handling user profile data and a mapper for object-object mapping.
    /// </summary>
    /// <param name="userProfileRepository">The repository to manage user profile persistence.</param>
    /// <param name="mapper">The mapper to handle object-object mapping.</param>
    public UserService(IUserProfileRepository userProfileRepository, IMapper mapper)
    {
        _userProfileRepository = userProfileRepository;
        _mapper = mapper;
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
    public async Task<UserProfileDto> GetProfileAsync(Guid userId)
    {
        var userProfile = await _userProfileRepository.GetByUserIdOrFailAsync(userId);

        return _mapper.Map<UserProfileDto>(userProfile);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var userProfile = await _userProfileRepository.GetByUserIdOrFailAsync(userId);

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