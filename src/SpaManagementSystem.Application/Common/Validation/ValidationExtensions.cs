using FluentValidation;

namespace SpaManagementSystem.Application.Common.Validation;

/// <summary>
/// Provides extension methods for <see cref="IRuleBuilder{T, TProperty}"/> to simplify adding common validation rules.
/// This static class extends the capabilities of rule builders used in FluentValidation to include custom validations
/// for phone numbers, passwords, and names, ensuring consistency and reuse across various parts of the application.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Adds validation rules for phone numbers.
    /// </summary>
    /// <param name="rule">The rule builder for a string property.</param>
    /// <returns>Configured rule builder options for validating phone numbers.</returns>
    /// <remarks>
    /// Ensures that the phone number is not null, not empty, and matches the specified pattern for digits only (9-15 digits).
    /// </remarks>
    public static IRuleBuilderOptions<T, string> MatchPhoneNumber<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotNull()
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\d{9,15}$").WithMessage("The phone number should consist of 9-15 digits");
        
    /// <summary>
    /// Adds validation rules for passwords.
    /// </summary>
    /// <param name="rule">The rule builder for a string property.</param>
    /// <returns>Configured rule builder options for validating passwords.</returns>
    /// <remarks>
    /// Ensures that the password is not null, not empty, and follows security guidelines:
    /// at least 8 characters long, includes at least one uppercase letter, one lowercase letter, one number,
    /// and one special character (!? *.).
    /// </remarks>
    public static IRuleBuilderOptions<T, string> MatchPassword<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotNull()
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("The password should be at least 8 characters long.")
            .Matches(@"[A-Z]+").WithMessage("The password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("The password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("The password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("The password must contain at least special character (!? *.).");

    /// <summary>
    /// Adds validation rules for names.
    /// </summary>
    /// <param name="rule">The rule builder for a string property.</param>
    /// <returns>Configured rule builder options for validating names.</returns>
    /// <remarks>
    /// Validates that the name only consists of letters and spaces. This is useful for ensuring that names are
    /// entered as expected without numerical or special characters.
    /// </remarks>
    public static IRuleBuilderOptions<T, string> MatchName<T>(this IRuleBuilder<T, string> rule)
        => rule
            .Matches(@"^[A-Za-z]+(?:\\s[A-Za-z]+)*$")
            .WithMessage("The name can only consist of letters and spaces");
}