using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
    {
        public ReturnBookCommandValidator()
        {
            RuleFor(command => command.User)
                .NotNull().WithMessage("User cannot be null.");

            RuleFor(command => command.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be a positive integer.");
        }
    }
}
