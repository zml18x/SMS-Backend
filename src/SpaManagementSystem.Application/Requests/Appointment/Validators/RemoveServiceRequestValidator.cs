using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Appointment.Validators;

public class RemoveServiceRequestValidator : AbstractValidator<RemoveServicesRequest>
{
    public RemoveServiceRequestValidator()
    {
        RuleFor(x => x.AppointmentServiceId)
            .ValidateId("AppointmentServiceId");
    }
}