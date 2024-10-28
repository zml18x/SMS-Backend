using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Application.Requests.Service;

namespace SpaManagementSystem.Application.Extensions;

public static class ServiceExtensions
{
    public static bool HasChanges(this Service existingService, UpdateServiceRequest request)
    {
        return existingService.Name != request.Name ||
               existingService.Code.ToUpper() != request.Code.ToUpper() ||
               existingService.Description != request.Description ||
               existingService.Price != request.Price ||
               existingService.TaxRate != request.TaxRate ||
               existingService.Duration != request.Duration ||
               existingService.ImgUrl != request.ImgUrl ||
               existingService.IsActive != request.IsActive;
    }
}