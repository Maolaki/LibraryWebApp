using FluentValidation;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class AddAuthorCommandValidator : AbstractValidator<AddAuthorCommand>
    {
        public AddAuthorCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .Length(1, 20).WithMessage("First name must be between 1 and 20 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .Length(1, 20).WithMessage("Last name must be between 1 and 20 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Date of birth is required.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Country)
                .NotNull().WithMessage("Country is required.")
                .IsInEnum()
                .WithMessage("Invalid country value.");
        }
    }
}
