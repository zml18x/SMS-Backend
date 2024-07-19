﻿using FluentValidation;
using SpaManagementSystem.Application.Common.Validation;

namespace SpaManagementSystem.Application.Requests.UserAccount.Validators
{
    /// <summary>
    /// Validator class for validating <see cref="ResetPasswordRequest"/> instances.
    /// </summary>
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetPasswordRequestValidator"/> class.
        /// </summary>
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.NewPassword)
                .MatchPassword();
            
            RuleFor(x => x.Token)
                .NotNull().WithMessage("Confirmation token is required")
                .NotEmpty().WithMessage("Confirmation token is required");
        }
    }
}