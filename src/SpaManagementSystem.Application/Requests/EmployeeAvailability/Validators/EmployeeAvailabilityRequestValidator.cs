using FluentValidation;

namespace SpaManagementSystem.Application.Requests.EmployeeAvailability.Validators;

public class EmployeeAvailabilityRequestValidator : AbstractValidator<EmployeeAvailabilityRequest>
{
    public EmployeeAvailabilityRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date cannot be in the past.");

        RuleFor(x => x.AvailabilityHours)
            .NotEmpty().WithMessage("Availability hours cannot be empty.");

        RuleForEach(x => x.AvailabilityHours).SetValidator(new AvailabilityHoursRequestValidator());
    }
}