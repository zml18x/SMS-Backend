using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Service;

namespace SpaManagementSystem.Application.Interfaces;

public interface ISalonServiceService
{
    public Task<ServiceDto> CreateServiceAsync(CreateServiceRequest request);
    public Task<ServiceDto> GetServiceByIdAsync(Guid serviceId);
    public Task<IEnumerable<ServiceDto>> GetServicesAsync(Guid salonId, string? code = null, string? name = null,
        bool? active = null);

    public Task<IEnumerable<EmployeeSummaryDto>> GetEmployeesAssignedToServiceAsync(Guid serviceId);
    public Task<OperationResult> UpdateServiceAsync(Guid serviceId, JsonPatchDocument<UpdateServiceRequest> patchDocument);
    public Task DeleteAsync(Guid serviceId);
}