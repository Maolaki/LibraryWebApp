using FluentValidation;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAllBooksByAuthorQueryValidator : AbstractValidator<GetAllBooksByAuthorQuery>
    {
        public GetAllBooksByAuthorQueryValidator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than 0.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");
        }
    }
}
