namespace SpaManagementSystem.Domain.Interfaces;

/// <summary>
/// Defines the standard operations to be performed on a repository for a specific type of entity.
/// This interface provides an abstraction over basic CRUD operations to ensure a decoupled design
/// and ease of unit testing by allowing different storage implementations.
/// </summary>
/// <typeparam name="TEntity">The type of entity this repository will manage. The entity must be a class.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation and contains the enumerable of entity.</returns>
    public Task<IEnumerable<TEntity>> GetAllAsync();
        
    /// <summary>
    /// Retrieves a single entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="entityId">The unique identifier of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation and contains the entity found,
    /// or null if no entity is found.</returns>
    public Task<TEntity?> GetByIdAsync(Guid entityId);

    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add to the repository.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task CreateAsync(TEntity entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update. The entity must already exist in the repository.</param>
    public void Update(TEntity entity);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete. The entity must exist in the repository.</param>
    public void Delete(TEntity entity);

    /// <summary>
    /// Saves all changes made in the context of the repository asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation,
    /// typically completing when all changes have been committed to the data store.</returns>
    public Task SaveChangesAsync();
}