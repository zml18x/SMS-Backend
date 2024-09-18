using AutoMapper;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Employee;

namespace SpaManagementSystem.Application.Services;

public class EmployeeService(IEmployeeRepository employeeRepository, ISalonRepository salonRepository, IMapper mapper,
    EmployeeBuilder employeeBuilder) : IEmployeeService
{
    public async Task AddEmployeeAsync(CreateEmployeeRequest request)
    {
        var salon = await salonRepository.GetWithEmployeesById(request.SalonId);
        if (salon == null)
            throw new NotFoundException($"Salon with ID {request.SalonId} was not found.");
        
        if (salon.Employees.Any(x => x.UserId == request.UserId))
            throw new InvalidOperationException($"Employee with UserId {request.UserId} is already assigned to the salon.");

        var employee = employeeBuilder
            .WithSalonId(request.SalonId)
            .WithUserId(request.UserId)
            .WithPosition(request.Position)
            .WithEmploymentStatus(request.EmploymentStatus)
            .WithCode(request.Code)
            .WithColor(request.Color)
            .WithHireDate(request.HireDate)
            .Build();

        var employeeProfile = employeeBuilder
            .WithFirstName(request.FirstName)
            .WithLastName(request.LastName)
            .WithGender(request.Gender)
            .WithDateOfBirth(request.DateOfBirth)
            .WithEmail(request.Email)
            .WithPhoneNumber(request.PhoneNumber)
            .BuildEmployeeProfile();
        
        employee.AddEmployeeProfile(employeeProfile);
        
        salon.AddEmployee(employee);
        
        await salonRepository.SaveChangesAsync();
    }

    public async Task<EmployeeDto> GetEmployeeByUserId(Guid userId)
    {
        var employee = await employeeRepository.GetByUserIdAsync(userId);
        if (employee == null)
            throw new NotFoundException($"Employee with user ID '{userId}' was not found.");

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> GetEmployeeById(Guid employeeId)
    {
        var employee = await employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
            throw new NotFoundException($"Employee with ID '{employeeId}' was not found.");

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDetailsDto> GetEmployeeDetailsByUserId(Guid userId)
    {
        var employee = await employeeRepository.GetWithProfileByUserIdAsync(userId);
        if (employee == null)
            throw new NotFoundException($"Employee with user ID '{userId}' was not found.");

        return mapper.Map<EmployeeDetailsDto>(employee);
    }
}