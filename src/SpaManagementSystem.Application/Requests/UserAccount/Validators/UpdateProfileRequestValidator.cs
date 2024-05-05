using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Provides validation for <see cref="UpdateProfileRequest"/> using FluentValidation.
    /// This class ensures that the data provided for updating a user's profile
    /// is valid and meets the application's standards for each field.
    /// </summary>
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProfileRequestValidator"/> class
        /// and defines the rules for validating the profile update request.
        /// </summary>
        public UpdateProfileRequestValidator()
        {
            When(x => x.FirstName != null, () =>
            {
                RuleFor(r => r.FirstName!)
                    .MatchName()
                    .Length(2 - 50).WithMessage("The first name can be 2 characters long and up to 50 characters long");
            });

            When(x => x.LastName != null, () =>
            {
                RuleFor(r => r.LastName!)
                    .MatchName()
                    .Length(2 - 50).WithMessage("The last name can be 2 characters long and up to 50 characters long");
            });

            When(x => x.Gender != null, () =>
            {
                RuleFor(r => r.DateOfBirth)
                    .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                    .WithMessage("Date of birth must be less than or equal to today's date");
            });

            When(x => x.DateOfBirth != null, () =>
            {
                RuleFor(r => r.Gender)
                    .IsEnumName(typeof(GenderType), false)
                    .WithMessage("Gender must be either 'male', 'female', or 'other'.");
            });
        }
    }
}