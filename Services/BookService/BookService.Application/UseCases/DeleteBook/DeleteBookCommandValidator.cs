using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(command => command.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be a positive integer.");
        }
    }
}
