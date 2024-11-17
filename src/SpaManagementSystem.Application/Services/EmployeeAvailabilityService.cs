using AutoMapper;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.ValueObjects;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.EmployeeAvailability;

namespace SpaManagementSystem.Application.Services;

public class EmployeeAvailabilityService(
    IEmployeeRepository employeeRepository,
    IRepository<EmployeeAvailability> availabilityRepository,
    EmployeeAvailabilityBuilder availabilityBuilder,
    IMapper mapper) : IEmployeeAvailabilityService
{
    public async Task<IEnumerable<EmployeeAvailabilityDto>> CreateAvailabilitiesByUserIdAsync(Guid userId, CreateAvailabilityRequest request)
    {
        var employee = await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetByUserIdAsync(userId));
        var newAvailabilities = await CreateAvailabilitiesAsync(employee.Id, request);

        return newAvailabilities;
    }

    public async Task<IEnumerable<EmployeeAvailabilityDto>> CreateAvailabilitiesByEmployeeIdAsync(Guid employeeId, CreateAvailabilityRequest request)
        => await CreateAvailabilitiesAsync(employeeId, request);

    public async Task<IEnumerable<EmployeeAvailabilityDto>> GetAvailabilitiesAsync(
        Guid employeeId,
        DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            throw new ArgumentException("Start date cannot be greater than end date.");

        var employee = await employeeRepository
            .GetOrThrowAsync(() => employeeRepository.GetWithAvailabilitiesByIdAsync(employeeId, startDate, endDate));

        return mapper.Map<IEnumerable<EmployeeAvailabilityDto>>(employee.EmployeeAvailabilities);
    }

    public async Task UpdateAvailabilityAsync(Guid availabilityId, UpdateAvailabilityRequest request)
    {
        var availability =
            await availabilityRepository.GetOrThrowAsync(() => availabilityRepository.GetByIdAsync(availabilityId));
        
        var newAvailableHours = request.AvailabilityHours
            .Select(hours => new AvailableHours(hours.Start, hours.End));

        availability.ClearAvailableHours();

        foreach (var availableHour in newAvailableHours)
            availability.AddAvailableHour(availableHour);

        await availabilityRepository.SaveChangesAsync();
    }
    
    public async Task DeleteAvailabilityAsync(Guid availabilityId)
    {
        var availability =
            await availabilityRepository.GetOrThrowAsync(() => availabilityRepository.GetByIdAsync(availabilityId));
        
        availabilityRepository.Delete(availability);
        await availabilityRepository.SaveChangesAsync();
    }
    
    public async Task DeleteAvailabilitiesInRangeForEmployeeAsync(Guid employeeId, DateOnly? startDate, DateOnly? endDate)
        => await DeleteAvailabilitiesInRangeAsync(employeeId, startDate, endDate);

    public async Task DeleteAvailabilitiesInRangeByUserIdAsync(Guid userId, DateOnly? startDate, DateOnly? endDate)
    {
        var employee = await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetByUserIdAsync(userId));

        await DeleteAvailabilitiesInRangeAsync(employee.Id, startDate, endDate);
    }
    
    private async Task DeleteAvailabilitiesInRangeAsync(Guid employeeId, DateOnly? startDate = null, DateOnly? endDate = null)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            throw new ArgumentException("Start date cannot be greater than end date.");
        
        await employeeRepository.GetOrThrowAsync(() => employeeRepository.GetWithAvailabilitiesByIdAsync(employeeId));
        
        await employeeRepository.DeleteAvailabilitiesInRange(employeeId, startDate, endDate);
        await employeeRepository.SaveChangesAsync();
    }
    
    private async Task<IEnumerable<EmployeeAvailabilityDto>> CreateAvailabilitiesAsync(Guid employeeId, CreateAvailabilityRequest request)
    {
        var employee = await employeeRepository
            .GetOrThrowAsync(() =>
                employeeRepository.GetWithAvailabilitiesByIdAsync(employeeId, DateOnly.FromDateTime(DateTime.UtcNow)));
        
        var newAvailabilities = new List<EmployeeAvailability>();
        
        foreach (var employeeAvailability in request.EmployeeAvailabilities)
        {
            if (employee.HasAvailabilityOnDate(employeeAvailability.Date))
                throw new InvalidOperationException(
                    $"Availability already exists for date {employeeAvailability.Date}.");

            var newAvailableHours = employeeAvailability.AvailabilityHours
                .Select(hours => new AvailableHours(hours.Start, hours.End));

            var newAvailability = availabilityBuilder
                .WithEmployeeId(employeeId)
                .WithDate(employeeAvailability.Date)
                .WithAvailableHours(newAvailableHours)
                .Build();

            employee.AddAvailability(newAvailability);
            newAvailabilities.Add(newAvailability);
        }

        await employeeRepository.SaveChangesAsync();
        
        return mapper.Map<IEnumerable<EmployeeAvailabilityDto>>(newAvailabilities);
    }
}