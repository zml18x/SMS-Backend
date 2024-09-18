using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    public Task<Employee?> GetByUserIdAsync(Guid userId);
    public Task<Employee?> GetWithProfileByUserIdAsync(Guid userId);
}