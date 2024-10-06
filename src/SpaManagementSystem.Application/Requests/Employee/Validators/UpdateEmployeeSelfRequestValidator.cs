using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Employee.Validators;

public class UpdateEmployeeSelfRequestValidator : AbstractValidator<UpdateEmployeeSelfRequest>
{
    public UpdateEmployeeSelfRequestValidator()
    {
        RuleFor(x => x.Color)
            .MatchHexColor();
        
        When(x => x.Notes != null, () =>
        {
            RuleFor(x => x.Notes!)
                .MatchEmployeeNotes();
        });
    }
}