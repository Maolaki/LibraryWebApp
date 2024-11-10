using FluentValidation;
using LibraryWebApp.AuthService.Application.Entities;

namespace LibraryWebApp.AuthService.Application.Validators
{
    public class AuthenticatedDTOValidator : AbstractValidator<AuthenticatedDTO>
    {
        public AuthenticatedDTOValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty().WithMessage("AccessToken is required.");
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("RefreshToken is required.");
        }
    }
}
