using FluentValidation;
using LibraryWebApp.AuthorService.Application.DTOs;

namespace LibraryWebApp.AuthorService.Application.Validators
{
    public class AuthorDTOValidator : AbstractValidator<AuthorDTO>
    {
        public AuthorDTOValidator()
        {
            RuleFor(author => author.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(1, 20).WithMessage("First name must be between 1 and 20 characters.");

            RuleFor(author => author.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(1, 20).WithMessage("Last name must be between 1 and 20 characters.");

            RuleFor(author => author.DateOfBirth)
                .LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Date of birth must be in the past.");

            RuleFor(author => author.Country)
                .NotNull().WithMessage("Country is required.");
        }
    }
}
