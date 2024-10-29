using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Product;

namespace SpaManagementSystem.Application.Interfaces;

public interface IProductService
{
    public Task<ProductDto> CreateProductAsync(CreateProductRequest request);
    public Task<ProductDto> GetProductByIdAsync(Guid productId);
    public Task<IEnumerable<ProductDto>> GetProductsAsync(Guid salonId, string? code = null, string? name = null,
        bool? active = null);
    public Task<OperationResult> UpdateProductAsync(Guid serviceId,
        JsonPatchDocument<UpdateProductRequest> patchDocument);
    public Task DeleteProductAsync(Guid serviceId);
}