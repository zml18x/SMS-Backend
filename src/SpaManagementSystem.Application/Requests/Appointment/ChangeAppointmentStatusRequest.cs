using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.Appointment;

public record ChangeAppointmentStatusRequest(AppointmentStatus Status);