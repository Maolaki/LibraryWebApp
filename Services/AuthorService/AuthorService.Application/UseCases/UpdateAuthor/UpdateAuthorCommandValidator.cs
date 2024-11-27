using FluentValidation;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than 0.");

            RuleFor(x => x.FirstName)
                .Length(1, 20)
                .WithMessage("First name must be between 1 and 20 characters.");

            RuleFor(x => x.LastName)
                .Length(1, 20)
                .WithMessage("Last name must be between 1 and 20 characters.");

            RuleFor(x => x.DateOfBirth)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Country)
                .IsInEnum()
                .WithMessage("Invalid country value.");
        }
    }
}
