using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="UpdateProfileRequest"/> instances.
    /// </summary>
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProfileRequestValidator"/> class.
        /// </summary>
        public UpdateProfileRequestValidator()
        {
            When(x => x.FirstName != null, () =>
            {
                RuleFor(x => x.FirstName!)
                    .MatchName()
                    .Length(2, 50).WithMessage("The first name can be 2 characters long and up to 50 characters long");
            });

            When(x => x.LastName != null, () =>
            {
                RuleFor(x => x.LastName!)
                    .MatchName()
                    .Length(2, 50).WithMessage("The last name can be 2 characters long and up to 50 characters long");
            });

            When(x => x.Gender != null, () =>
            {
                RuleFor(x => x.DateOfBirth)
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                    .WithMessage("Date of birth must be less than or equal to today's date");
            });

            When(x => x.DateOfBirth != null, () =>
            {
                RuleFor(x => x.Gender)
                    .IsEnumName(typeof(GenderType), false)
                    .WithMessage("Gender must be either 'male', 'female', or 'other'.");
            });
        }
    }
}