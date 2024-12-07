using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Employee.Validators;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.SalonId)
            .ValidateId("SalonId");

        RuleFor(x => x.UserId)
            .ValidateId("UserId");

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