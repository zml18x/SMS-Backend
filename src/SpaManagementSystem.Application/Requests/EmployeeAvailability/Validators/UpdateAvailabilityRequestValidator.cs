using FluentValidation;

namespace SpaManagementSystem.Application.Requests.EmployeeAvailability.Validators;

public class UpdateAvailabilityRequestValidator : AbstractValidator<UpdateAvailabilityRequest>
{
    public UpdateAvailabilityRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date cannot be in the past.");
        
        RuleForEach(x => x.AvailabilityHours).SetValidator(new AvailabilityHoursRequestValidator());
    }
}