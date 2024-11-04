using FluentValidation;
using LibraryWebApp.AuthService.Domain.Entities;

public class AuthenticatedDTOValidator : AbstractValidator<AuthenticatedDTO>
{
    public AuthenticatedDTOValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty().WithMessage("AccessToken is required.");
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("RefreshToken is required.");
    }
}
