using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Employee;

namespace SpaManagementSystem.Application.Interfaces;

public interface IEmployeeService
{
    public Task<EmployeeDetailsDto> CreateEmployeeAsync(CreateEmployeeRequest request);
    public Task<OperationResult> UpdateEmployeeAsync(Guid employeeId, JsonPatchDocument<UpdateEmployeeRequest> patchDocument);
    public Task<OperationResult> UpdateEmployeeAsync(Guid userId, JsonPatchDocument<UpdateEmployeeSelfRequest> patchDocument);
    public Task<OperationResult> UpdateEmployeeProfileAsync(Guid employeeId, JsonPatchDocument<UpdateEmployeeProfileRequest> patchDocument);
    public Task<OperationResult> UpdateEmployeeProfileAsync(Guid userId, JsonPatchDocument<UpdateEmployeeProfileSelfRequest> patchDocument);
    public Task DeleteEmployeeAsync(Guid employeeId);
    public Task<EmployeeDetailsDto> GetEmployeeWithProfileByIdAsync(Guid employeeId);
    public Task<EmployeeDetailsDto> GetEmployeeWithProfileByUserIdAsync(Guid userId);
    public Task<IEnumerable<EmployeeSummaryDto>> GetEmployeesAsync(Guid salonId, string? code = null,
        string? firstName = null, string? lastName = null, EmploymentStatus? status = null);
    public Task AssignServiceToEmployeeAsync(Guid employeeId, Guid serviceId);
    public Task RemoveServiceFromEmployeeAsync(Guid employeeId, Guid serviceId);
    public Task<IEnumerable<ServiceDto>> GetEmployeeServices(Guid employeeId);
}