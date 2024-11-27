using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetAllBooksWithFiltersQueryValidator : AbstractValidator<GetAllBooksWithFiltersQuery>
    {
        public GetAllBooksWithFiltersQueryValidator()
        {
            RuleFor(query => query.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(query => query.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100.");

            RuleFor(query => query.Title)
                .MaximumLength(100)
                .WithMessage("Title must not exceed 100 characters.");

            RuleFor(query => query.Genre)
                .IsInEnum()
                .WithMessage("Genre must be a valid value.");

            RuleFor(query => query.AuthorId)
                .GreaterThan(0)
                .When(query => query.AuthorId.HasValue)
                .WithMessage("AuthorId must be a positive integer.");
        }
    }
}
