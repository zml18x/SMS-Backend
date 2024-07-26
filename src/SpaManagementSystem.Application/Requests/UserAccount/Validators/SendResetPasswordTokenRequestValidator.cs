﻿using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators;

/// <summary>
/// Validator class for validating <see cref="SendResetPasswordTokenRequest"/> instances.
/// </summary>
public class SendResetPasswordTokenRequestValidator : AbstractValidator<SendResetPasswordTokenRequest>
{
    public SendResetPasswordTokenRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Email is required.")
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.NewPassword)
            .MatchPassword();
    }
}