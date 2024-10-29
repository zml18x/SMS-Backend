using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface IProductRepository : IRepository<Product> , IUniqueCodeRepository
{
    public Task<IEnumerable<Product>> GetProductsAsync(Guid salonId, string? code = null, string? name = null,
        bool? active = null);
}