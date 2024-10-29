using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Application.Common.Helpers;
using SpaManagementSystem.Application.Requests.Product;
using SpaManagementSystem.Application.Requests.Product.Validators;

namespace SpaManagementSystem.Application.Services;

public class ProductService(
    IProductRepository productRepository,
    ISalonRepository salonRepository,
    ProductBuilder productBuilder,
    IMapper mapper)
    : IProductService
{
    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request)
    {
        var isCodeTaken = await productRepository.IsExistsAsync(request.SalonId, request.Code);
        if (isCodeTaken)
            throw new InvalidOperationException($"Service with code {request.Code} already exist.");

        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(request.SalonId));

        var product = productBuilder
            .WithSalonId(request.SalonId)
            .WithName(request.Name)
            .WithCode(request.Code)
            .WithDescription(request.Description)
            .WithPurchasePrice(request.PurchasePrice)
            .WithPurchaseTaxRate(request.PurchaseTaxRate)
            .WithSalePrice(request.SalePrice)
            .WithSaleTaxRate(request.SaleTaxRate)
            .WithStockQuantity(request.StockQuantity)
            .WithMinimumStockQuantity(request.MinimumStockQuantity)
            .WithUnitOfMeasure(request.UnitOfMeasure)
            .WithImgUrl(request.ImgUrl)
            .Build();

        salon.AddProduct(product);
        await salonRepository.SaveChangesAsync();

        return mapper.Map<ProductDto>(product);
    }
    
    public async Task<ProductDto> GetProductByIdAsync(Guid productId)
    {
        var service = await productRepository.GetOrThrowAsync(() => productRepository.GetByIdAsync(productId));

        return mapper.Map<ProductDto>(service);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(Guid salonId, string? code = null, string? name = null, bool? active = null)
    {
        await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));
        var services = await productRepository.GetProductsAsync(salonId, code, name, active);
        
        return mapper.Map<IEnumerable<ProductDto>>(services);
    }
    
    public async Task<OperationResult> UpdateProductAsync(Guid serviceId, JsonPatchDocument<UpdateProductRequest> patchDocument)
    {
        var existingProduct = await productRepository.GetOrThrowAsync(() => productRepository.GetByIdAsync(serviceId));
        
        var request = mapper.Map<UpdateProductRequest>(existingProduct);
        
        return await new PatchUpdateHelper().ApplyPatchAndUpdateAsync(
            patchDocument,
            existingProduct,
            request,
            new UpdateProductRequestValidator(),
            (p, r) => p.UpdateProduct(r.Name, r.Code, r.Description, r.PurchasePrice,
                r.PurchaseTaxRate, r.SalePrice, r.SaleTaxRate, r.StockQuantity, r.MinimumStockQuantity, r.UnitOfMeasure,
                r.IsActive, r.ImgUrl),
            p => new ProductSpecification().IsSatisfiedBy(p),
            (p, r) => p.HasChanges(r),
            productRepository
        );
    }

    public async Task DeleteProductAsync(Guid serviceId)
    {
        var product = await productRepository.GetOrThrowAsync(() => productRepository.GetByIdAsync(serviceId));
        
        productRepository.Delete(product);
        await productRepository.SaveChangesAsync();
    }
}