using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface ISalonRepository : IRepository<Salon>
{
    public Task<Salon> GetByUserIdAsync(Guid userId);
    public Task<IEnumerable<Salon>> GetAllByUserIdAsync(Guid userId);
}