using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;

namespace SpaManagementSystem.Application.Extensions.RepositoryExtensions;

/// <summary>
/// Provides extension methods for the <see cref="IUserProfileRepository"/> interface.
/// </summary>
public static class UserProfileRepositoryExtensions
{
    /// <summary>
    /// Retrieves a user profile by its unique user identifier.
    /// </summary>
    /// <param name="repository">The user profile repository.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="UserProfile"/> entity.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the user ID is empty.</exception>
    /// <exception cref="NotFoundException">Thrown when the user profile is not found.</exception>
    public static async Task<UserProfile> GetByUserIdOrFailAsync(this IUserProfileRepository repository, Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("The user id cannot be empty", nameof(userId));

        var userProfile = await repository.GetByUserIdOrFailAsync(userId);
        if (userProfile == null)
            throw new NotFoundException($"The user profile for user with id {userId} was not found.");

        return userProfile;
    }
}