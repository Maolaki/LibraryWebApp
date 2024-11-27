using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetAvailableCopiesQueryValidator : AbstractValidator<GetAvailableCopiesQuery>
    {
        public GetAvailableCopiesQueryValidator()
        {
            RuleFor(query => query.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(10, 17).WithMessage("ISBN must be between 10 and 17 characters.");
        }
    }
}
