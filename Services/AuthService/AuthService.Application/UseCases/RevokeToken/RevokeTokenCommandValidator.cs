using FluentValidation;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("RefreshToken cannot be null or empty.");

            RuleFor(x => x.User)
                .NotNull()
                .WithMessage("User cannot be null.");
        }
    }
}
