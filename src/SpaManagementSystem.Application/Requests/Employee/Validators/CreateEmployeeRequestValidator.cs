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
            .NotEmpty().WithMessage("Position is required.")
            .MaximumLength(100).WithMessage("Position cannot be longer than 100 characters.");

        RuleFor(x => x.EmploymentStatus)
            .IsInEnum().WithMessage("Invalid employment status.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(8).WithMessage("Code cannot be longer than 8 characters.")
            .Matches("^[A-Za-z0-9]+$").WithMessage("Code can only contain alphanumeric characters.");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Color is required.")
            .Matches("^#([A-Fa-f0-9]{6})$").WithMessage("Color must be in HEX format.");

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Hire date cannot be in the future.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-50)))
            .WithMessage("Hire date cannot be more than 50 years in the past.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender type.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Date of birth must be in the past.")
            .Must(BeAtLeast16YearsOld).WithMessage("Employee must be at least 16 years old.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

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