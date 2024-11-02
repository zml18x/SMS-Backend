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
using SpaManagementSystem.Application.Requests.Service;
using SpaManagementSystem.Application.Requests.Service.Validators;


namespace SpaManagementSystem.Application.Services;

public class SalonServiceService(ISalonRepository salonRepository, IServiceRepository serviceRepository,
    ServiceBuilder serviceBuilder, IMapper mapper) : ISalonServiceService
{
    public async Task<ServiceDto> CreateServiceAsync(CreateServiceRequest request)
    {
        var isCodeTaken = await serviceRepository.IsExistsAsync(request.SalonId, request.Code);
        if (isCodeTaken)
            throw new InvalidOperationException($"Service with code {request.Code} already exist.");
        
        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(request.SalonId));
        
        var service = serviceBuilder
            .WithSalonId(request.SalonId)
            .WithName(request.Name)
            .WithCode(request.Code)
            .WithDescription(request.Description)
            .WithPrice(request.Price)
            .WithTaxRate(request.TaxRate)
            .WithDuration(request.Duration)
            .WithImgUrl(request.ImgUrl)
            .Build();
        
        salon.AddService(service);
        await salonRepository.SaveChangesAsync();

        return mapper.Map<ServiceDto>(service);
    }
    
    public async Task<IEnumerable<ServiceDto>> GetServicesAsync(Guid salonId, string? code = null, string? name = null, bool? active = null)
    {
        await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));
        var services = await serviceRepository.GetServicesAsync(salonId, code, name, active);
        
        return mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    public async Task<ServiceDto> GetServiceByIdAsync(Guid serviceId)
    {
        var service = await serviceRepository.GetOrThrowAsync(() => serviceRepository.GetByIdAsync(serviceId));

        return mapper.Map<ServiceDto>(service);
    }

    public async Task<IEnumerable<EmployeeSummaryDto>> GetEmployeesAssignedToServiceAsync(Guid serviceId)
    {
        var service = await serviceRepository.GetOrThrowAsync(() => serviceRepository.GetWithEmployeesAsync(serviceId));

        return mapper.Map<IEnumerable<EmployeeSummaryDto>>(service.Employees);
    }

    public async Task<OperationResult> UpdateServiceAsync(Guid serviceId, JsonPatchDocument<UpdateServiceRequest> patchDocument)
    {
        var existingService = await serviceRepository.GetOrThrowAsync(() => serviceRepository.GetByIdAsync(serviceId));
        
        var request = mapper.Map<UpdateServiceRequest>(existingService);
        
        return await new PatchUpdateHelper().ApplyPatchAndUpdateAsync(
            patchDocument,
            existingService,
            request,
            new UpdateServiceRequestValidator(),
            (s, r) => s.UpdateService(r.Name, r.Code, r.Description, r.Price, r.TaxRate, r.Duration, r.ImgUrl, r.IsActive),
            s => new ServiceSpecification().IsSatisfiedBy(s),
            (s, r) => s.HasChanges(r),
            serviceRepository
        );
    }
    
    public async Task DeleteAsync(Guid serviceId)
    {
        var service = await serviceRepository.GetOrThrowAsync(() => serviceRepository.GetByIdAsync(serviceId));
        
        serviceRepository.Delete(service);
        await serviceRepository.SaveChangesAsync();
    }
}