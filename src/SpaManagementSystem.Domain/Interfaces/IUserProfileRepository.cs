using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces
{
    /// <summary>
    /// Defines a repository specifically for managing user profile entities.
    /// This interface extends the generic IRepository interface by adding functionality
    /// to retrieve user profiles based on user IDs.
    /// </summary>
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        /// <summary>
        /// Retrieves a user profile based on a user's unique identifier asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose profile is to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation and returns the user profile associated with the specified user ID,
        /// or null if no profile is found.
        /// </returns>
        public Task<UserProfile?> GetByUserIdAsync(Guid userId);
    }
}