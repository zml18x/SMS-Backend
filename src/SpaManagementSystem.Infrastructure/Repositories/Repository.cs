using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;

namespace SpaManagementSystem.Infrastructure.Repositories;

/// <summary>
/// A generic repository implementation that handles CRUD operations for the specified TEntity type
/// within the context of the Spa Management System.
/// This class provides a layer of abstraction over the Entity Framework Core operations,
/// thereby centralizing the data access logic.
/// </summary>
/// <typeparam name="TEntity">The type of the entity this repository manages, which must be a class.</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly SmsDbContext _context;



    /// <summary>
    /// Initializes a new instance of the Repository with the specified database context.
    /// </summary>
    /// <param name="context">The database context this repository will operate on,
    /// managing the lifecycle of the TEntity instances.</param>
    public Repository(SmsDbContext context)
    {
        _context = context;
    }
        
        
        
    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().ToListAsync();

    /// <inheritdoc/>
    public async Task<TEntity?> GetByIdAsync(Guid entityId)
        => await _context.Set<TEntity>().FindAsync(entityId);

    /// <inheritdoc/>
    public async Task CreateAsync(TEntity entity)
    {
        CheckEntityIsNull(entity);
        await _context.Set<TEntity>().AddAsync(entity);
    }

    /// <inheritdoc/>
    public void Update(TEntity entity)
    {
        CheckEntityIsNull(entity);
        _context.Update(entity);
    }

    /// <inheritdoc/>
    public void Delete(TEntity entity)
    {
        CheckEntityIsNull(entity);
        _context.Remove(entity);
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    /// <summary>
    /// Helper method to check if the entity is null, ensuring that operations do not proceed with null entities.
    /// </summary>
    /// <param name="entity">The entity to check.</param>
    /// <exception cref="ArgumentNullException">Thrown when the entity is null,
    /// indicating an attempt to operate on a non-existent object.</exception>
    private void CheckEntityIsNull(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} cannot be null");
    }
}