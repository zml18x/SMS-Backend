using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.Employee.Validators;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeRequestValidator()
    {
        RuleFor(x => x.SalonId)
            .NotEmpty().WithMessage("SalonId is required.")
            .Must(g => g != Guid.Empty).WithMessage("SalonId must be a valid non-empty GUID.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(g => g != Guid.Empty).WithMessage("UserId must be a valid non-empty GUID.");

        RuleFor(x => x.Position)
            .MatchEmployeePosition();

        RuleFor(x => x.EmploymentStatus)
            .MatchEmploymentStatus();

        RuleFor(x => x.Code)
            .MatchEmployeeCode();

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
            .LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Date of birth must be in the past.")
            .Must(BeAtLeast16YearsOld).WithMessage("Employee must be at least 16 years old.");

        RuleFor(x => x.Email)
            .MatchEmail();

        RuleFor(x => x.PhoneNumber)
            .MatchPhoneNumber();
    }
    
    private bool BeAtLeast16YearsOld(DateOnly dateOfBirth)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Today);
        if (dateOfBirth >= currentDate)
            return false;

        var age = currentDate.Year - dateOfBirth.Year;

        return age switch
        {
            > 16 => true,
            < 16 => false,
            _ => currentDate.Month > dateOfBirth.Month ||
                 (currentDate.Month == dateOfBirth.Month && currentDate.Day >= dateOfBirth.Day)
        };
    }
}