using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class CheckoutBookCommandValidator : AbstractValidator<CheckoutBookCommand>
    {
        public CheckoutBookCommandValidator()
        {
            RuleFor(command => command.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be a positive integer.");

            RuleFor(command => command.User)
                .NotNull()
                .WithMessage("User cannot be null.");
        }
    }
}
