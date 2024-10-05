using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Employee;

namespace SpaManagementSystem.Application.Interfaces;

public interface IEmployeeService
{
    public Task<EmployeeDetailsDto> AddEmployeeAsync(CreateEmployeeRequest request);
    public Task<EmployeeDto> GetEmployeeByUserIdAsync(Guid userId);
    public Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId);
    public Task<EmployeeDetailsDto> GetEmployeeDetailsByUserIdAsync(Guid userId);
    public Task<EmployeeDetailsDto> GetEmployeeDetailsByIdAsync(Guid employeeId);
    public Task<EmployeeDto> GetEmployeeByCodeAsync(string employeeCode);
    public Task<EmployeeDetailsDto> GetEmployeeDetailsByCodeAsync(string employeeCode);
    public Task<OperationResult> UpdateEmployeeAsync(Guid employeeId, JsonPatchDocument<UpdateEmployeeRequest> patchDocument);
    public Task<OperationResult> UpdateEmployeeProfileAsync(Guid employeeId, JsonPatchDocument<UpdateEmployeeProfileRequest> patchDocument);
}