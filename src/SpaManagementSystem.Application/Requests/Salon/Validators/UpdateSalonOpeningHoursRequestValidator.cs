using FluentValidation;
using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Requests.Salon.Validators;

public class UpdateSalonOpeningHoursRequestValidator : AbstractValidator<UpdateSalonOpeningHoursRequest>
{
    public UpdateSalonOpeningHoursRequestValidator()
    {
        RuleForEach(x => x.OpeningHours)
            .NotEmpty().WithMessage("Opening hours cannot be empty.")
            .Must(HaveValidDayOfWeek)
            .WithMessage((request, openingHours) => $"Invalid day of week ({openingHours.DayOfWeek}).")
            .Must(HaveValidOpeningAndClosingTimes)
            .WithMessage((request, openingHours) => $"Invalid opening or closing times for day ({openingHours.DayOfWeek}). Opening time must be before the closing time.");
    }

    private bool HaveValidDayOfWeek(UpdateSalonOpeningHoursRequest request, OpeningHoursDto openingHours)
        => Enum.IsDefined(typeof(DayOfWeek), openingHours.DayOfWeek);

    private bool HaveValidOpeningAndClosingTimes(UpdateSalonOpeningHoursRequest request, OpeningHoursDto openingHours)
        => openingHours.OpeningTime < openingHours.ClosingTime;
}