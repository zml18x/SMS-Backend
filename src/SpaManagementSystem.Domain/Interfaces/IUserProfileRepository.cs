using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces
{
    /// <summary>
    /// Extends the generic IRepository interface to include additional functionality for managing user profiles.
    /// Provides methods specific to the UserProfile entity, including retrieving UserProfile by user ID.
    /// Inherits from IRepository&lt;UserProfile&gt;.
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