using FluentValidation;

namespace SpaManagementSystem.Application.Requests.Auth.Validators;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Access token cannot be empty.")
            .Must(BeAValidJwt).WithMessage("Invalid access token format.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token cannot be empty.");
    }

    private bool BeAValidJwt(string token)
        => token.Split('.').Length == 3;
}