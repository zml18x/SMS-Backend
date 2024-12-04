namespace SpaManagementSystem.Application.Requests.Appointment;

public record CreateAppointmentRequest(
    Guid SalonId,
    Guid EmployeeId,
    Guid CustomerId,
    DateOnly Date,
    DateTime StartTime,
    DateTime EndTime,
    string? Notes,
    IEnumerable<CreateAppointmentServiceRequest> Services);