using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

/// <summary>
/// Represents a repository  for managing user profile.
/// This repository implements both the general repository operations provided through the Repository base class
/// and the specific operations defined in the <see cref="IUserProfileRepository"/>.
/// </summary>
public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
{
    private readonly SmsDbContext _context;



    /// <summary>
    /// Initializes a new instance of the <see cref="UserProfileRepository"/> class with the specified context.
    /// </summary>
    /// <param name="context">The database context used for salon data operations.</param>
    public UserProfileRepository(SmsDbContext context) : base(context)
    {
        _context = context;
    }



    /// <inheritdoc />
    /// <exception cref="NotFoundException">Thrown when no user profile is found for the specified <paramref name="userId"/>.</exception>
    public async Task<UserProfile> GetByUserIdAsync(Guid userId)
    {
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
        if (userProfile == null)
            throw new NotFoundException($"User profile for user id {userId} was not found.");

        return userProfile;
    }
}