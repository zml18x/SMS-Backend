using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.EmployeeAvailability;

namespace SpaManagementSystem.Application.Interfaces;

public interface IEmployeeAvailabilityService
{
    public Task<IEnumerable<EmployeeAvailabilityDto>> CreateAvailabilitiesByUserIdAsync(Guid userId, CreateAvailabilityRequest request);
    public Task<IEnumerable<EmployeeAvailabilityDto>> CreateAvailabilitiesByEmployeeIdAsync(Guid employeeId, CreateAvailabilityRequest request);
    public Task<IEnumerable<EmployeeAvailabilityDto>> GetAvailabilitiesAsync(Guid employeeId, DateOnly? startDate = null,
        DateOnly? endDate = null);
    public Task UpdateAvailabilityAsync(Guid availabilityId, UpdateAvailabilityRequest request);
    public Task DeleteAvailabilityAsync(Guid availabilityId);
    public Task DeleteAvailabilitiesInRangeForEmployeeAsync(Guid employeeId, DateOnly? startDate, DateOnly? endDate);
    public Task DeleteAvailabilitiesInRangeByUserIdAsync(Guid userId, DateOnly? startDate, DateOnly? endDate);
}