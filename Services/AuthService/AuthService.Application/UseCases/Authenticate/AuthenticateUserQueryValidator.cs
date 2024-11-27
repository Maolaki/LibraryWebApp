using FluentValidation;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class AuthenticateUserQueryValidator : AbstractValidator<AuthenticateUserQuery>
    {
        public AuthenticateUserQueryValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("Login is required.")
                .MinimumLength(3)
                .WithMessage("Login must be at least 3 characters long.")
                .MaximumLength(20)
                .WithMessage("Login cannot exceed 20 characters.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 8 characters long.");
        }
    }
}
