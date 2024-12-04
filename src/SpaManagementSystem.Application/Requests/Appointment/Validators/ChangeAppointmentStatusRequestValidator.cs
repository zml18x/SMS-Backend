using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Appointment.Validators;

public class ChangeAppointmentStatusRequestValidator : AbstractValidator<ChangeAppointmentStatusRequest>
{
    public ChangeAppointmentStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .MatchAppointmentStatus();
    }
}