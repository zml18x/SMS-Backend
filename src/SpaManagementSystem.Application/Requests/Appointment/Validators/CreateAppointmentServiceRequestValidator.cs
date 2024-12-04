using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Appointment.Validators;

public class CreateAppointmentServiceRequestValidator : AbstractValidator<CreateAppointmentServiceRequest>
{
    public CreateAppointmentServiceRequestValidator()
    {
        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage("ServiceID is required.")
            .Must(g => g != Guid.Empty).WithMessage("ServiceID must be a valid non-empty GUID.");

        RuleFor(x => x.Price)
            .ValidatePrice();
    }
}