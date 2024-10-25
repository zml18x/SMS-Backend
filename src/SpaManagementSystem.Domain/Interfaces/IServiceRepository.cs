using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface IServiceRepository : IRepository<Service>
{
    public Task<bool> IsExistsAsync(Guid salonId, string code);
    public Task<IEnumerable<Service>> GetServicesAsync(Guid salonId, string? code = null, string? name = null, bool? active = null);
}