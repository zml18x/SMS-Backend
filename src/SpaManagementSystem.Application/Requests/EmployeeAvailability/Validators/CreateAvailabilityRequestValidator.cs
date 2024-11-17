using FluentValidation;

namespace SpaManagementSystem.Application.Requests.EmployeeAvailability.Validators;

public class CreateAvailabilityRequestValidator : AbstractValidator<CreateAvailabilityRequest>
{
    public CreateAvailabilityRequestValidator()
    {
        RuleFor(x => x.EmployeeAvailabilities)
            .NotEmpty().WithMessage("Employee availabilities cannot be empty.")
            .Must(HasNoDuplicateDates).WithMessage("Duplicate dates are not allowed.");

        RuleForEach(x => x.EmployeeAvailabilities).SetValidator(new EmployeeAvailabilityRequestValidator());
    }

    private bool HasNoDuplicateDates(IEnumerable<EmployeeAvailabilityRequest> employeeAvailabilities)
        => employeeAvailabilities
            .GroupBy(e => e.Date)
            .All(g => g.Count() == 1);
}