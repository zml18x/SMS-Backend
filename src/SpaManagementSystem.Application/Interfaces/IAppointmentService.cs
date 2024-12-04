using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Appointment;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Interfaces;

public interface IAppointmentService
{
    public Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentRequest request);
    public Task<AppointmentDto> GetAppointmentByIdAsync(Guid appointmentId);
    public Task<IEnumerable<AppointmentDto>> GetAppointmentsBySalonIdAsync(Guid salonId,
        DateOnly? startDate, DateOnly? endDate, AppointmentStatus? status);
    public Task<IEnumerable<AppointmentDto>> GetAppointmentsByEmployeeIdAsync(Guid employeeId,
        DateOnly? startDate, DateOnly? endDate, AppointmentStatus? status);
    public Task<IEnumerable<AppointmentDto>> GetAppointmentsByCustomerIdAsync(Guid customerId, DateOnly? startDate,
        DateOnly? endDate, AppointmentStatus? status);
    public Task UpdateStatusAsync(Guid appointmentId, AppointmentStatus status);
    public Task UpdateAppointmentAsync(Guid appointmentId, UpdateAppointmentRequest request);
    public Task DeleteAppointmentAsync(Guid appointmentId);
}