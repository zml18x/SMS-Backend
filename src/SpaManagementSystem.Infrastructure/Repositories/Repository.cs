using Microsoft.EntityFrameworkCore;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Infrastructure.Data.Context;
using SpaManagementSystem.Application.Exceptions;

namespace SpaManagementSystem.Infrastructure.Repositories;

public class Repository<TEntity>(SmsDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await context.Set<TEntity>().ToListAsync();
    
    public async Task<TEntity> GetByIdAsync(Guid entityId)
    {
        var entity = await context.Set<TEntity>().FindAsync(entityId);
        if(entity == null)
            throw new NotFoundException($"{typeof(TEntity).Name} with ID {entityId} was not found.");
    
        return entity;
    }
    
    public async Task CreateAsync(TEntity entity)
    {
        CheckEntityIsNull(entity);
        await context.Set<TEntity>().AddAsync(entity);
    }
    
    public void Update(TEntity entity)
    {
        CheckEntityIsNull(entity);
        context.Update(entity);
    }
    
    public void Delete(TEntity entity)
    {
        CheckEntityIsNull(entity);
        context.Remove(entity);
    }
    
    public async Task SaveChangesAsync()
        => await context.SaveChangesAsync();
    
    private void CheckEntityIsNull(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} cannot be null");
    }
}