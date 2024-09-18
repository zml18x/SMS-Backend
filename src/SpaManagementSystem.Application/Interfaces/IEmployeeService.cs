using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Employee;

namespace SpaManagementSystem.Application.Interfaces;

public interface IEmployeeService
{
    public Task AddEmployeeAsync(CreateEmployeeRequest request);
    public Task<EmployeeDto> GetEmployeeByUserId(Guid userId);
    public Task<EmployeeDto> GetEmployeeById(Guid employeeId);
    public Task<EmployeeDetailsDto> GetEmployeeDetailsByUserId(Guid userId);
}