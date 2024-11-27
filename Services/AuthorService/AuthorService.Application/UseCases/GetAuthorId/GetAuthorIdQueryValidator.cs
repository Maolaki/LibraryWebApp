using FluentValidation;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAuthorIdQueryValidator : AbstractValidator<GetAuthorIdQuery>
    {
        public GetAuthorIdQueryValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("FirstName is required.")
                .Length(1, 20).WithMessage("First name must be between 1 and 20 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastName is required.")
                .Length(1, 20).WithMessage("Last name must be between 1 and 20 characters.");
        }
    }
}
