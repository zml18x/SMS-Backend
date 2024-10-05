using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Employee.Validators;

public class UpdateEmployeeProfileRequestValidator : AbstractValidator<UpdateEmployeeProfileRequest>
{
    public UpdateEmployeeProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .MatchFirstName();

        RuleFor(x => x.LastName)
            .MatchLastName();

        RuleFor(x => x.Gender)
            .MatchGender();

        RuleFor(x => x.DateOfBirth)
            .MatchEmployeeDateOfBirth();

        RuleFor(x => x.Email)
            .MatchEmail();

        RuleFor(x => x.PhoneNumber)
            .MatchPhoneNumber();
    }
}