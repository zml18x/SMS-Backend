using FluentValidation;

namespace SpaManagementSystem.Application.Requests.EmployeeAvailability.Validators;

public class AvailabilityHoursRequestValidator : AbstractValidator<AvailabilityHoursRequest>
{
    public AvailabilityHoursRequestValidator()
    {
        RuleFor(x => x.Start)
            .NotEmpty().WithMessage("Start time is required.")
            .Must(BeAValidTime).WithMessage("Start time must be a valid time between 00:00 and 23:59.")
            .LessThan(x => x.End).WithMessage("Start time must be before end time.");

        RuleFor(x => x.End)
            .NotEmpty().WithMessage("End time is required.")
            .Must(BeAValidTime).WithMessage("End time must be a valid time between 00:00 and 23:59.");
    }

    private bool BeAValidTime(TimeSpan time)
        => time >= TimeSpan.Zero && time < TimeSpan.FromDays(1);
}