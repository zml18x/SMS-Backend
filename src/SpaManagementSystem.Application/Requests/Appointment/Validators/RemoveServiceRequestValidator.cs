using FluentValidation;

namespace SpaManagementSystem.Application.Requests.Appointment.Validators;

public class RemoveServiceRequestValidator : AbstractValidator<RemoveServicesRequest>
{
    public RemoveServiceRequestValidator()
    {
        RuleFor(x => x.AppointmentServiceId)
            .NotEmpty().WithMessage("AppointmentServiceId is required.")
            .Must(g => g != Guid.Empty).WithMessage("AppointmentServiceId must be a valid non-empty GUID.");
    }
}