using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Employee;

namespace SpaManagementSystem.Application.Interfaces;

public interface IEmployeeService
{
    public Task AddEmployeeAsync(CreateEmployeeRequest request);
    public Task<EmployeeDto> GetEmployeeByUserIdAsync(Guid userId);
    public Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId);
    public Task<EmployeeDetailsDto> GetEmployeeDetailsByUserIdAsync(Guid userId);
    public Task<EmployeeDetailsDto> GetEmployeeDetailsByIdAsync(Guid employeeId);
    public Task<EmployeeDto> GetEmployeeByCodeAsync(string employeeCode);
    public Task<EmployeeDetailsDto> GetEmployeeDetailsByCodeAsync(string employeeCode);
}