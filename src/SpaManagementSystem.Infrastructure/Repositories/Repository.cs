using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;
using SpaManagementSystem.Application.Exceptions;

namespace SpaManagementSystem.Infrastructure.Repositories;

/// <summary>
/// Provides a generic repository for managing entity data in the context of the application.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly SmsDbContext _context;


    
    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class with the specified context.
    /// </summary>
    /// <param name="context">The database context used for data operations.</param>
    public Repository(SmsDbContext context)
    {
        _context = context;
    }
        
        
        
    /// <inheritdoc/>
    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _context.Set<TEntity>().ToListAsync();

    /// <inheritdoc/>
    /// <exception cref="NotFoundException">Thrown when an entity with the specified <paramref name="entityId"/> is not found.</exception>
    public async Task<TEntity> GetByIdAsync(Guid entityId)
    {
        var entity = await _context.Set<TEntity>().FindAsync(entityId);
        if(entity == null)
            throw new NotFoundException($"{typeof(TEntity).Name} with ID {entityId} was not found.");
    
        return entity;
    }

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