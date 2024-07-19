using FluentValidation;
using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Requests.Salon.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="UpdateSalonOpeningHoursRequest"/> instances.
    /// </summary>
    public class UpdateSalonOpeningHoursRequestValidator : AbstractValidator<UpdateSalonOpeningHoursRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSalonOpeningHoursRequestValidator"/> class.
        /// </summary>
        public UpdateSalonOpeningHoursRequestValidator()
        {
            // Validate the salon opening hours
            RuleForEach(x => x.OpeningHours)
                .NotEmpty().WithMessage("Opening hours cannot be empty.")
                .Must(HaveValidDayOfWeek)
                .WithMessage((request, openingHours) => $"Invalid day of week ({openingHours.DayOfWeek}).")
                .Must(HaveValidOpeningAndClosingTimes)
                .WithMessage((request, openingHours) => $"Invalid opening or closing times for day ({openingHours.DayOfWeek}). Opening time must be before the closing time.");
        }
        
        /// <summary>
        /// Validates if the <see cref="DayOfWeek"/> in the <see cref="OpeningHoursDto"/> is a valid enum value.
        /// </summary>
        /// <param name="request">The <see cref="UpdateSalonOpeningHoursRequest"/> being validated.</param>
        /// <param name="openingHours">The <see cref="OpeningHoursDto"/> instance being validated.</param>
        /// <returns>Returns true if the <see cref="DayOfWeek"/> is valid; otherwise, false.</returns>
        private bool HaveValidDayOfWeek(UpdateSalonOpeningHoursRequest request, OpeningHoursDto openingHours)
            => Enum.IsDefined(typeof(DayOfWeek), openingHours.DayOfWeek);
        
        /// <summary>
        /// Validates if the opening time is before the closing time in the <see cref="OpeningHoursDto"/>.
        /// </summary>
        /// <param name="request">The <see cref="UpdateSalonOpeningHoursRequest"/> being validated.</param>
        /// <param name="openingHours">The <see cref="OpeningHoursDto"/> instance being validated.</param>
        /// <returns>Returns true if the opening time is before the closing time; otherwise, false.</returns>
        private bool HaveValidOpeningAndClosingTimes(UpdateSalonOpeningHoursRequest request, OpeningHoursDto openingHours)
            => openingHours.OpeningTime < openingHours.ClosingTime;
    }
}