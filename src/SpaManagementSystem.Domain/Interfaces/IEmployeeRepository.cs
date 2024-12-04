using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface IEmployeeRepository : IRepository<Employee> , IUniqueCodeRepository
{
    public Task<Employee?> GetByUserIdAsync(Guid userId);
    public Task<Employee?> GetWithProfileByUserIdAsync(Guid userId);
    public Task<Employee?> GetWithProfileByIdAsync(Guid employeeId);
    public Task<Employee?> GetWithServicesByIdAsync(Guid employeeId);
    public Task<Employee?> GetWithAvailabilitiesByIdAsync(Guid employeeId, DateOnly? startDate = null,
        DateOnly? endDate = null);
    public Task<IEnumerable<Employee>> GetEmployeesAsync(Guid salonId, string? code = null,
        string? firstName = null, string? lastName = null, EmploymentStatus? status = null);
    public Task DeleteAvailabilitiesInRange(Guid employeeId, DateOnly? startDate = null, DateOnly? endDate = null);
    public Task<bool> IsEmployeeAvailableForAppointmentAsync(Guid employeeId, DateOnly date, DateTime startTime,
        DateTime endTime, Guid? appointmentId = null);
}