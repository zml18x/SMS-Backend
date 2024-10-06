using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Employee.Validators;

public class UpdateEmployeeProfileSelfRequestValidator : AbstractValidator<UpdateEmployeeProfileSelfRequest>
{
    public UpdateEmployeeProfileSelfRequestValidator()
    {
        RuleFor(x => x.Email)
            .MatchEmail();

        RuleFor(x => x.PhoneNumber)
            .MatchPhoneNumber();
    }
}