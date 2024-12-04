using FluentValidation;

namespace SpaManagementSystem.Application.Requests.Appointment.Validators;

public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x.SalonId)
            .NotEmpty().WithMessage("SalonId is required.")
            .Must(g => g != Guid.Empty).WithMessage("SalonId must be a valid non-empty GUID.");
        
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("EmployeeId is required.")
            .Must(g => g != Guid.Empty).WithMessage("EmployeeId must be a valid non-empty GUID.");
        
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.")
            .Must(g => g != Guid.Empty).WithMessage("CustomerId must be a valid non-empty GUID.");
        
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .Must(d => d >= DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Date cannot be in the past.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("StartTime is required.")
            .Must((request, startTime) => startTime < request.EndTime)
            .WithMessage("StartTime must be earlier than EndTime.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("EndTime is required.")
            .Must((request, endTime) => endTime > request.StartTime)
            .WithMessage("EndTime must be later than StartTime.");

        RuleFor(x => x.Services)
            .NotEmpty().WithMessage("Services collection is required.")
            .Must(s => s.Any()).WithMessage("At least one service must be provided.")
            .ForEach(serviceRule =>
            {
                serviceRule.SetValidator(new CreateAppointmentServiceRequestValidator());
            });
        
        When(x => !string.IsNullOrEmpty(x.Notes), () =>
        {
            RuleFor(x => x.Notes!)
                .MaximumLength(500).WithMessage("Notes cannot be longer than 500 characters");
        });
    }
}