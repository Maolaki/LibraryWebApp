using FluentValidation;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAllAuthorsQueryValidator : AbstractValidator<GetAllAuthorsQuery>
    {
        public GetAllAuthorsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");
        }
    }
}
