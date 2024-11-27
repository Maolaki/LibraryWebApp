using FluentValidation;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
    {
        public DeleteAuthorCommandValidator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than 0.");
        }
    }
}
