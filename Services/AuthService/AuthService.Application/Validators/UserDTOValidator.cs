using FluentValidation;
using LibraryWebApp.AuthService.Application.DTOs;

namespace LibraryWebApp.AuthService.Application.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username cannot be empty.")
                .Length(3, 15).WithMessage("Username must be between 3 and 15 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .Length(8, 25).WithMessage("Password must be between 8 and 25 characters.")
                .Matches(@"[A-Za-z]").WithMessage("Password must contain at least one letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
        }
    }
}
