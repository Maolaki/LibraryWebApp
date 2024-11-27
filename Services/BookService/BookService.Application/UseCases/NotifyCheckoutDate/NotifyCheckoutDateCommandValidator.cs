using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class NotifyCheckoutDateCommandValidator : AbstractValidator<NotifyCheckoutDateCommand>
    {
        public NotifyCheckoutDateCommandValidator()
        {
            RuleFor(command => command.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be a positive integer.");
        }
    }
}
