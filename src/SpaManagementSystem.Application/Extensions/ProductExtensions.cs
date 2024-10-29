using SpaManagementSystem.Application.Requests.Product;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Application.Extensions;

public static class ProductExtensions
{
    public static bool HasChanges(this Product existingService, UpdateProductRequest request)
    {
        return existingService.Name != request.Name ||
               existingService.Code.ToUpper() != request.Code.ToUpper() ||
               existingService.Description != request.Description ||
               existingService.PurchasePrice != request.PurchasePrice ||
               existingService.PurchaseTaxRate != request.PurchaseTaxRate ||
               existingService.SalePrice != request.SalePrice ||
               existingService.SaleTaxRate != request.SaleTaxRate ||
               existingService.StockQuantity != request.StockQuantity ||
               existingService.MinimumStockQuantity != request.MinimumStockQuantity ||
               existingService.UnitOfMeasure != request.UnitOfMeasure ||
               existingService.IsActive != request.IsActive ||
               existingService.ImgUrl != request.ImgUrl;
    }
}