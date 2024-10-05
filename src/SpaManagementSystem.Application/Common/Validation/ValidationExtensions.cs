﻿using FluentValidation;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Common.Validation;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MatchEmail<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    
    public static IRuleBuilderOptions<T, string> MatchPhoneNumber<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\d{9,15}$").WithMessage("The phone number should consist of 9-15 digits");
    
    public static IRuleBuilderOptions<T, string> MatchPassword<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("The password should be at least 8 characters long.")
            .Matches(@"[A-Z]+").WithMessage("The password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("The password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("The password must contain at least one number.")
            .Matches(@"[\!\?\*\.]+").WithMessage("The password must contain at least special character (!? *.).");
    
    public static IRuleBuilderOptions<T, string> MatchName<T>(this IRuleBuilder<T, string> rule)
        => rule
            .Matches(@"^[A-Za-zÀ-ÿ'.-]+(?:\s[A-Za-zÀ-ÿ'.-]+)*$")
            .WithMessage("The name can only consist of letters, spaces, apostrophes, hyphens, and periods");

    public static IRuleBuilderOptions<T, string> MatchFirstName<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty().WithMessage("First name is required.")
            .MatchName()
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");

    public static IRuleBuilderOptions<T, string> MatchLastName<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty().WithMessage("Last name is required.")
            .MatchName()
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

    public static IRuleBuilderOptions<T, GenderType> MatchGender<T>(this IRuleBuilder<T, GenderType> rule)
        => rule
            .IsInEnum().WithMessage("Invalid gender type.");
    
    public static IRuleBuilderOptions<T, DateOnly> MatchEmployeeDateOfBirth<T>(this IRuleBuilder<T, DateOnly> rule)
        => rule
            .LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Date of birth must be in the past.")
            .Must(BeAtLeast16YearsOld).WithMessage("Employee must be at least 16 years old.");
    
    public static IRuleBuilderOptions<T, string> MatchEmployeeCode<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(8).WithMessage("Code cannot be longer than 8 characters.")
            .Matches("^[A-Za-z0-9]+$").WithMessage("Code can only contain alphanumeric characters.");

    public static IRuleBuilderOptions<T, string> MatchEmployeePosition<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty().WithMessage("Position is required.")
            .MaximumLength(100).WithMessage("Position cannot be longer than 100 characters.");
    
    public static IRuleBuilderOptions<T, EmploymentStatus> MatchEmploymentStatus<T>(this IRuleBuilder<T, EmploymentStatus> rule)
        => rule
            .IsInEnum().WithMessage("Invalid employment status.");
    
    public static IRuleBuilderOptions<T, string> MatchHexColor<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty().WithMessage("Color is required.")
            .Matches("^#([A-Fa-f0-9]{6})$").WithMessage("Color must be in HEX format.");

    public static IRuleBuilderOptions<T, DateOnly> MatchHireDate<T>(this IRuleBuilder<T, DateOnly> rule)
        => rule
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Hire date cannot be in the future.")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Today.AddYears(-50)))
            .WithMessage("Hire date cannot be more than 50 years in the past.");

    public static IRuleBuilderOptions<T, string> MatchEmployeeNotes<T>(this IRuleBuilder<T, string> rule)
        => rule
            .MaximumLength(1000).WithMessage("Notes cannot be longer than 1000 characters");
    private static bool BeAtLeast16YearsOld(DateOnly dateOfBirth)
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