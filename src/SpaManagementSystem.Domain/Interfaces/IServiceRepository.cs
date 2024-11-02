using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface IServiceRepository : IRepository<Service> , IUniqueCodeRepository
{
    public Task<Service?> GetWithEmployeesAsync(Guid serviceId);
    public Task<IEnumerable<Service>> GetServicesAsync(Guid salonId, string? code = null, string? name = null, bool? active = null);
}