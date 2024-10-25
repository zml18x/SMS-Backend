using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface ISalonRepository : IRepository<Salon>
{
    public Task<Salon> GetByUserIdAsync(Guid userId);
    public Task<Salon?> GetWithEmployeesByIdAsync(Guid salonId);
    public Task<Salon?> GetWithServicesAsync(Guid salonId);
    public Task<IEnumerable<Salon>> GetAllByUserIdAsync(Guid userId);
}