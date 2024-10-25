using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Employee.Validators;

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequestValidator()
    {
        RuleFor(x => x.Position)
            .MatchEmployeePosition();

        RuleFor(x => x.EmploymentStatus)
            .MatchEmploymentStatus();

        RuleFor(x => x.Code)
            .MatchCode();

        RuleFor(x => x.Color)
            .MatchHexColor();

        RuleFor(x => x.HireDate)
            .MatchHireDate();
        
        When(x => x.Notes != null, () =>
        {
            RuleFor(x => x.Notes!)
                .MatchEmployeeNotes();
        });
    }
}