namespace SpaManagementSystem.Application.Requests.Appointment;

public record CreateAppointmentServiceRequest(
    Guid ServiceId,
    decimal Price);