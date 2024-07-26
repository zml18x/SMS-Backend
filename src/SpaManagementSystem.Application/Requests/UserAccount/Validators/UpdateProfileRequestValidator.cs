using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators;

/// <summary>
/// Validator class for validating <see cref="UpdateProfileRequest"/> instances.
/// </summary>
public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName!)
            .NotNull().WithMessage("The first name cannot be null")
            .MatchName()
            .Length(2, 50).WithMessage("The first name can be 2 characters long and up to 50 characters long");

        RuleFor(x => x.LastName!)
            .NotNull().WithMessage("The last name cannot be null")
            .MatchName()
            .Length(2, 50).WithMessage("The last name can be 2 characters long and up to 50 characters long");

        RuleFor(x => x.DateOfBirth)
            .NotNull().WithMessage("The date of birth cannot be null")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth must be less than or equal to today's date");

        RuleFor(x => x.Gender)
            .NotNull().WithMessage("The gender cannot be null")
            .IsEnumName(typeof(GenderType), false)
            .WithMessage("Gender must be either 'male', 'female', or 'other'.");
    }
}