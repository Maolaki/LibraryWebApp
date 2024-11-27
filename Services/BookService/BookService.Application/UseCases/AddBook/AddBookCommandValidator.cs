using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class AddBookCommandValidator : AbstractValidator<AddBookCommand>
    {
        public AddBookCommandValidator()
        {
            RuleFor(command => command.AuthorId)
                .GreaterThan(0)
                .WithMessage("AuthorId must be a positive integer.");

            RuleFor(command => command.ISBN)
                .NotEmpty()
                .WithMessage("ISBN is required.")
                .Length(10, 17)
                .WithMessage("ISBN must be between 10 and 17 characters.");

            RuleFor(command => command.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .Length(1, 100)
                .WithMessage("Title must be between 1 and 100 characters.");

            RuleFor(command => command.Description)
                .MaximumLength(1000)
                .WithMessage("Description must not exceed 1000 characters.");

            RuleFor(command => command.Genre)
                .IsInEnum()
                .WithMessage("Genre must be a valid value.");
        }
    }
}
