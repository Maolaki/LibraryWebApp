using FluentValidation;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAuthorQueryValidator : AbstractValidator<GetAuthorQuery>
    {
        public GetAuthorQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");
        }
    }
}
