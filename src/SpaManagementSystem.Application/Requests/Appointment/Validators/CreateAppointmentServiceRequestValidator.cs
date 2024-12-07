using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Appointment.Validators;

public class CreateAppointmentServiceRequestValidator : AbstractValidator<CreateAppointmentServiceRequest>
{
    public CreateAppointmentServiceRequestValidator()
    {
        RuleFor(x => x.ServiceId)
            .ValidateId("ServiceId");

        RuleFor(x => x.Price)
            .ValidatePrice();
    }
}