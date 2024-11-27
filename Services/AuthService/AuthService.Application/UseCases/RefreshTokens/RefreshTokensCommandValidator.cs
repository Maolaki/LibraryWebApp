using FluentValidation;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class RefreshTokensCommandValidator : AbstractValidator<RefreshTokensCommand>
    {
        public RefreshTokensCommandValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty()
                .WithMessage("Access token is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required.");
        }
    }
}
