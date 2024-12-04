using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Interfaces;

public interface IAppointmentRepository : IRepository<Appointment>
{
    public Task<(bool salonExists, bool employeeExists, bool customerExists)> ValidateAppointmentEntitiesAsync(
        Guid salonId, Guid employeeId, Guid customerId);

    public Task<IEnumerable<Appointment>> GetAppointmentsBySalonIdAsync(Guid salonId, DateOnly? startDate = null,
        DateOnly? endDate = null, AppointmentStatus? status = null);

    public Task<IEnumerable<Appointment>> GetAppointmentsByEmployeeIdAsync(Guid employeeId, DateOnly? startDate = null,
        DateOnly? endDate = null, AppointmentStatus? status = null);
    
    public Task<IEnumerable<Appointment>> GetAppointmentsByCustomerIdAsync(Guid customerId,
        DateOnly? startDate = null, DateOnly? endDate = null, AppointmentStatus? status = null);
}