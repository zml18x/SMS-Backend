using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Employee;
using SpaManagementSystem.Application.Requests.Employee.Validators;

namespace SpaManagementSystem.Application.Services;

public class EmployeeService(IEmployeeRepository employeeRepository, ISalonRepository salonRepository,
    IMapper mapper, EmployeeBuilder employeeBuilder) : IEmployeeService
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

    public async Task<OperationResult> UpdateEmployeeAsync(Guid employeeId, JsonPatchDocument<UpdateEmployeeRequest> patchDocument)
    {
        var existingEmployee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByIdAsync(employeeId));

        var request = mapper.Map<UpdateEmployeeRequest>(existingEmployee);
        
        patchDocument.ApplyTo(request);

        var requestValidationResult = await new UpdateEmployeeRequestValidator().ValidateAsync(request);
        if (!requestValidationResult.IsValid)
        {
            var errors = requestValidationResult.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key, 
                    group => group.Select(error => error.ErrorMessage).ToArray()
                );

            return OperationResult.ValidationFailed(errors);
        }
        
        if (!existingEmployee.HasChanges(request))
            return OperationResult.NoChanges();

        var isUpdated = existingEmployee.UpdateEmployee(request.Position, request.EmploymentStatus, request.Code,
            request.Color, request.HireDate, request.Notes);
        
        if (!isUpdated) 
            return OperationResult.NoChanges();
        
        var validationResult = new EmployeeSpecification().IsSatisfiedBy(existingEmployee);
        if (!validationResult.IsValid)
            throw new InvalidOperationException($"Update failed: {string.Join(", ", validationResult.Errors)}");
        
        await employeeRepository.SaveChangesAsync();

        return OperationResult.Success();
    }

    public async Task<OperationResult> UpdateEmployeeProfileAsync(Guid employeeId, JsonPatchDocument<UpdateEmployeeProfileRequest> patchDocument)
    {
        var existingEmployee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByIdAsync(employeeId));
        
        var existingProfile = existingEmployee.Profile;
        
        var request = mapper.Map<UpdateEmployeeProfileRequest>(existingProfile);
        
        patchDocument.ApplyTo(request);

        var requestValidationResult = await new UpdateEmployeeProfileRequestValidator().ValidateAsync(request);
        if (!requestValidationResult.IsValid)
        {
            var errors = requestValidationResult.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key, 
                    group => group.Select(error => error.ErrorMessage).ToArray()
                );

            return OperationResult.ValidationFailed(errors);
        }

        if (!existingProfile.HasChanges(request))
            return OperationResult.NoChanges();

        var isUpdated = existingEmployee.Profile.UpdateEmployeeProfile(request.FirstName, request.LastName, request.Gender,
            request.DateOfBirth, request.Email, request.PhoneNumber);
        
        if (!isUpdated) 
            return OperationResult.NoChanges();
        
        var validationResult = new EmployeeProfileSpecification().IsSatisfiedBy(existingEmployee.Profile);
        if (!validationResult.IsValid)
            throw new InvalidOperationException($"Update failed: {string.Join(", ", validationResult.Errors)}");
            
        existingEmployee.UpdateTimestamp();
            
        await employeeRepository.SaveChangesAsync();

        return OperationResult.Success();
    }
}