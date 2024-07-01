using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Represents a repository specifically tailored for managing user profile entities.
    /// This repository implements both the general repository operations provided through the Repository base class
    /// and the specific operations defined in the IUserProfileRepository interface.
    /// </summary>
    public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
    {
        private readonly SmsDbContext _context;



        /// <summary>
        /// Initializes a new instance of the UserProfileRepository using the specified database context.
        /// This constructor ensures the repository is ready to manage UserProfile entities by passing the context
        /// to the base repository class.
        /// </summary>
        /// <param name="context">The database context used for UserProfile entity operations.</param>
        public UserProfileRepository(SmsDbContext context) : base(context)
        {
            _context = context;
        }


        
        /// <inheritdoc />
        public async Task<UserProfile?> GetByUserIdAsync(Guid userId)
            => await Task.FromResult(await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId));
    }
}