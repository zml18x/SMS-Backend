using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Employee.Validators;

public class EmployeeCodeValidator : AbstractValidator<string>
{
    public EmployeeCodeValidator()
    {
        RuleFor(code => code)
            .MatchCode();
    }
}