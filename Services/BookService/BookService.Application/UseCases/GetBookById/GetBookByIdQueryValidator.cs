using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetBookByIdQueryValidator : AbstractValidator<GetBookByIdQuery>
    {
        public GetBookByIdQueryValidator()
        {
            RuleFor(query => query.Id)
                .GreaterThan(0).WithMessage("Id must be a positive integer.");
        }
    }
}
