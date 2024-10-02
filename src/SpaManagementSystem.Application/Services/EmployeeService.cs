using AutoMapper;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions.RepositoryExtensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Employee;

namespace SpaManagementSystem.Application.Services;

public class EmployeeService(
    IEmployeeRepository employeeRepository,
    ISalonRepository salonRepository,
    IMapper mapper,
    EmployeeBuilder employeeBuilder) : IEmployeeService
{
    public async Task<EmployeeDetailsDto> AddEmployeeAsync(CreateEmployeeRequest request)
    {
        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetWithEmployeesById(request.SalonId));

        if (salon.Employees.Any(x => x.UserId == request.UserId))
            throw new InvalidOperationException(
                $"Employee with UserId {request.UserId} is already assigned to the salon.");

        if (salon.Employees.Any(x => x.Code.Equals(request.Code, StringComparison.CurrentCultureIgnoreCase)))
            throw new InvalidOperationException($"Employee with code {request.Code} already exist.");

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

        return mapper.Map<EmployeeDetailsDto>(employee);
    }

    public async Task<EmployeeDto> GetEmployeeByUserIdAsync(Guid userId)
    {
        var employee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetByUserIdAsync(userId));

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId)
    {
        var employee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetByIdAsync(employeeId));

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDetailsDto> GetEmployeeDetailsByIdAsync(Guid employeeId)
    {
        var employee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByIdAsync(employeeId));

        return mapper.Map<EmployeeDetailsDto>(employee);
    }

    public async Task<EmployeeDetailsDto> GetEmployeeDetailsByUserIdAsync(Guid userId)
    {
        var employee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByUserIdAsync(userId));

        return mapper.Map<EmployeeDetailsDto>(employee);
    }

    public async Task<EmployeeDto> GetEmployeeByCodeAsync(string employeeCode)
    {
        var employee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetByCodeAsync(employeeCode));

        return mapper.Map<EmployeeDto>(employee);
    }

    public async Task<EmployeeDetailsDto> GetEmployeeDetailsByCodeAsync(string employeeCode)
    {
        var employee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByCodeAsync(employeeCode));

        return mapper.Map<EmployeeDetailsDto>(employee);
    }

    public async Task UpdateEmployee(Guid employeeId, UpdateEmployeeRequest request)
    {
        var employee = await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetByIdAsync(employeeId));

        var isUpdated = employee.UpdateEmployee(request.Position, request.EmploymentStatus, request.Code, request.Color,
            request.HireDate, request.Notes);

        if (isUpdated)
        {
            var validationResult = new EmployeeSpecification().IsSatisfiedBy(employee);
            if (!validationResult.IsValid)
                throw new InvalidOperationException($"Update failed: {string.Join(", ", validationResult.Errors)}");
            
            await salonRepository.SaveChangesAsync();
        }
    }

    public bool HasChanges(EmployeeDto existingEmployee, UpdateEmployeeRequest request)
        => existingEmployee.Position != request.Position ||
           existingEmployee.EmploymentStatus != request.EmploymentStatus ||
           existingEmployee.Code != request.Code ||
           existingEmployee.Color != request.Color ||
           existingEmployee.HireDate != request.HireDate ||
           existingEmployee.Notes != request.Notes;
}