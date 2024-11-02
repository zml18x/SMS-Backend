using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Application.Common.Helpers;
using SpaManagementSystem.Application.Requests.Employee;
using SpaManagementSystem.Application.Requests.Employee.Validators;

namespace SpaManagementSystem.Application.Services;

public class EmployeeService(
    IEmployeeRepository employeeRepository,
    ISalonRepository salonRepository,
    IServiceRepository serviceRepository,
    IMapper mapper,
    EmployeeBuilder employeeBuilder) : IEmployeeService
{
    public async Task<EmployeeDetailsDto> CreateEmployeeAsync(CreateEmployeeRequest request)
    {
        var employee = await employeeRepository.GetByUserIdAsync(request.UserId);
        if (employee != null)
            throw new InvalidOperationException(
                $"Employee with UserId {request.UserId} is already assigned to the salon.");
        
        var isCodeTaken = await employeeRepository.IsExistsAsync(request.SalonId, request.Code);
        if (isCodeTaken)
            throw new InvalidOperationException($"Employee with code {request.Code} already exist.");
        
        var salon = await salonRepository.GetOrThrowAsync(() =>
            salonRepository.GetWithEmployeesByIdAsync(request.SalonId));

         employee = employeeBuilder
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
    
    public async Task<OperationResult> UpdateEmployeeAsync(Guid employeeId,
        JsonPatchDocument<UpdateEmployeeRequest> patchDocument)
    {
        var existingEmployee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByIdAsync(employeeId));

        var request = mapper.Map<UpdateEmployeeRequest>(existingEmployee);

        return await new PatchUpdateHelper().ApplyPatchAndUpdateAsync(
            patchDocument,
            existingEmployee,
            request,
            new UpdateEmployeeRequestValidator(),
            (e, r) => e.UpdateEmployee(r.Position, r.EmploymentStatus, r.Code, r.Color, r.HireDate, r.Notes),
            e => new EmployeeSpecification().IsSatisfiedBy(e),
            (e, r) => e.HasChanges(r),
            employeeRepository
        );
    }

    public async Task<OperationResult> UpdateEmployeeAsync(Guid userId,
        JsonPatchDocument<UpdateEmployeeSelfRequest> patchDocument)
    {
        var existingEmployee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetByUserIdAsync(userId));

        var request = mapper.Map<UpdateEmployeeSelfRequest>(existingEmployee);

        return await new PatchUpdateHelper().ApplyPatchAndUpdateAsync(
            patchDocument,
            existingEmployee,
            request,
            new UpdateEmployeeSelfRequestValidator(),
            (e, r) => e.UpdateEmployee(r.Color, r.Notes),
            e => new EmployeeSpecification().IsSatisfiedBy(e),
            (e, r) => e.HasChanges(r),
            employeeRepository
        );
    }

    public async Task<OperationResult> UpdateEmployeeProfileAsync(Guid employeeId,
        JsonPatchDocument<UpdateEmployeeProfileRequest> patchDocument)
    {
        var existingEmployee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByIdAsync(employeeId));

        var existingProfile = existingEmployee.Profile;

        var request = mapper.Map<UpdateEmployeeProfileRequest>(existingProfile);

        return await new PatchUpdateHelper().ApplyPatchAndUpdateAsync(
            patchDocument,
            existingEmployee,
            request,
            new UpdateEmployeeProfileRequestValidator(),
            (e, r) => e.Profile.UpdateEmployeeProfile(r.FirstName, r.LastName, r.Gender, r.DateOfBirth, r.Email,
                r.PhoneNumber),
            e => new EmployeeProfileSpecification().IsSatisfiedBy(e.Profile),
            (e, r) => e.Profile.HasChanges(r),
            employeeRepository
        );
    }

    public async Task<OperationResult> UpdateEmployeeProfileAsync(Guid userId,
        JsonPatchDocument<UpdateEmployeeProfileSelfRequest> patchDocument)
    {
        var existingEmployee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByUserIdAsync(userId));

        var existingProfile = existingEmployee.Profile;

        var request = mapper.Map<UpdateEmployeeProfileSelfRequest>(existingProfile);

        return await new PatchUpdateHelper().ApplyPatchAndUpdateAsync(
            patchDocument,
            existingEmployee,
            request,
            new UpdateEmployeeProfileSelfRequestValidator(),
            (e, r) => e.Profile.UpdateEmployeeProfile(r.Email, r.PhoneNumber),
            e => new EmployeeProfileSpecification().IsSatisfiedBy(e.Profile),
            (e, r) => e.Profile.HasChanges(r),
            employeeRepository
        );
    }
    
    public async Task DeleteEmployeeAsync(Guid employeeId)
    {
        var employee = await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetByIdAsync(employeeId));
        
        employeeRepository.Delete(employee);
        await employeeRepository.SaveChangesAsync();
    }
    
    public async Task<EmployeeDetailsDto> GetEmployeeWithProfileByIdAsync(Guid employeeId)
    {
        var employee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByIdAsync(employeeId));

        return mapper.Map<EmployeeDetailsDto>(employee);
    }
    
    public async Task<EmployeeDetailsDto> GetEmployeeWithProfileByUserIdAsync(Guid userId)
    {
        var employee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithProfileByUserIdAsync(userId));

        return mapper.Map<EmployeeDetailsDto>(employee);
    }
    
    public async Task<IEnumerable<EmployeeSummaryDto>> GetEmployeesAsync(Guid salonId, string? code = null,
        string? firstName = null, string? lastName = null, EmploymentStatus? status = null)
    {
        var employees = await employeeRepository.GetEmployeesAsync(salonId, code, firstName, lastName, status);
        
        return mapper.Map<IEnumerable<EmployeeSummaryDto>>(employees);
    }
    
    public async Task AssignServiceToEmployeeAsync(Guid employeeId, Guid serviceId)
    {
        var employee = await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetByIdAsync(employeeId));
        var service = await serviceRepository.GetOrThrowAsync(() => serviceRepository.GetByIdAsync(serviceId));
        
        if(service.SalonId != employee.SalonId)
            throw new InvalidOperationException(
                $"Cannot assign service with id {serviceId} to employee with id {employeeId} because the service belongs" +
                $" to a different salon (service salon id: {service.SalonId}, employee salon id: {employee.SalonId}).");
        
        employee.AddService(service);
        await employeeRepository.SaveChangesAsync();
    }

    public async Task RemoveServiceFromEmployeeAsync(Guid employeeId, Guid serviceId)
    {
        var employee = await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetWithServicesByIdAsync(employeeId));
        var service = await serviceRepository.GetOrThrowAsync(() => serviceRepository.GetByIdAsync(serviceId));
        
        employee.RemoveService(service);
        await employeeRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<ServiceDto>> GetEmployeeServices(Guid employeeId)
    {
        var employee = await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetWithServicesByIdAsync(employeeId));
        
        return mapper.Map<IEnumerable<ServiceDto>>(employee.Services);
    }
}