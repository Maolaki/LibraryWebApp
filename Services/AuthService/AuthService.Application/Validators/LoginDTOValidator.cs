using FluentValidation;
using LibraryWebApp.AuthService.Application.DTOs;

namespace LibraryWebApp.AuthService.Application.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Login cannot be empty.")
                .Length(3, 50).WithMessage("Login must be between 3 and 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .Length(8, 25).WithMessage("Password must be between 8 and 25 characters.")
                .Matches(@"[A-Za-z]").WithMessage("Password must contain at least one letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
        }
    }
}
