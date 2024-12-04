namespace SpaManagementSystem.Application.Requests.Appointment;

public record UpdateAppointmentRequest(
    Guid EmployeeId,
    Guid CustomerId,
    DateOnly Date,
    DateTime StartTime,
    DateTime EndTime,
    string? Notes,
    IEnumerable<CreateAppointmentServiceRequest> AddedServices,
    IEnumerable<RemoveServicesRequest> RemovedServices);