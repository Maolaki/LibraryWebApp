using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetBookImageQueryValidator : AbstractValidator<GetBookImageQuery>
    {
        public GetBookImageQueryValidator()
        {
            RuleFor(query => query.BookId)
                .GreaterThan(0).WithMessage("BookId must be a positive integer.");
        }
    }
}
