using FluentValidation;

namespace SpaManagementSystem.Application.Requests.Salon.Validators;

public class OpeningHoursRequestValidator : AbstractValidator<OpeningHoursRequest>
{
    public OpeningHoursRequestValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .NotNull().WithMessage("Day of week cannot be empty.")
            .IsInEnum().WithMessage(x => $"Invalid day of week ({x.DayOfWeek}).");
        
        RuleFor(x => x.OpeningTime)
            .NotEqual(TimeSpan.Zero)
            .WithMessage("Opening time cannot be the default value.");

        RuleFor(x => x.ClosingTime)
            .NotEqual(TimeSpan.Zero)
            .WithMessage("Closing time cannot be the default value.");
        
        RuleFor(x => x)
            .Must(x => x.ClosingTime > x.OpeningTime)
            .WithMessage("Closing time must be after opening time.");
    }
}