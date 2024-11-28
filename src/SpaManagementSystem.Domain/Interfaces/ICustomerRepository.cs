using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    public Task<Customer?> GetByPhoneNumberAsync(Guid salonId, string phoneNumber);
    public Task<Customer?> GetByEmailAsync(Guid salonId, string email);
    public Task<IEnumerable<Customer>> GetCustomersAsync(Guid salonId, string? firstName = null,
        string? lastName = null, string? phoneNumber = null, string? email = null, bool? isActive = null);
}