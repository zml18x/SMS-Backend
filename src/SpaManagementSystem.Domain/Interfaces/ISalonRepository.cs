using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

/// <summary>
/// Extends the generic IRepository interface to include additional functionality for managing salons.
/// Provides methods specific to the Salon entity, including retrieving salons by user ID.
/// Inherits from IRepository&lt;Salon&gt;.
/// </summary>
public interface ISalonRepository : IRepository<Salon>
{
    /// <summary>
    /// Asynchronously retrieves a salon by the associated user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A Task that represents the asynchronous operation and returns the salon  associated with the specified user id
    /// or null if no salon is found.</returns>
    public Task<Salon?> GetByUserIdAsync(Guid userId);
        
    /// <summary>
    /// Asynchronously retrieves a salon with its details by the specified salon ID.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon.</param>
    /// <returns>A Task that represents the asynchronous operation and returns the salon with its details
    /// associated with the specified salon ID or null if no salon is found.</returns>
    public Task<Salon?> GetWithDetailsByIdAsync(Guid salonId);
        
    /// <summary>
    /// Asynchronously retrieves all salons associated with a specific user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A Task that represents the asynchronous operation and returns the collection of salons
    /// associated with the specified user id.</returns>
    public Task<IEnumerable<Salon>> GetAllByUserIdAsync(Guid userId);
}