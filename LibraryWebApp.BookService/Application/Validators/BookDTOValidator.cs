namespace LibraryWebApp.BookService.Application.Validators
{
    using FluentValidation;
    using LibraryWebApp.BookService.Application.DTOs;

    public class BookDTOValidator : AbstractValidator<BookDTO>
    {
        public BookDTOValidator()
        {
            RuleFor(book => book.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(10, 13).WithMessage("ISBN must be between 10 and 13 characters.");

            RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 100).WithMessage("Title must be between 1 and 100 characters.");

            RuleFor(book => book.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(book => book.Genre)
                .IsInEnum().WithMessage("Genre must be a valid value.");

            RuleFor(book => book.AuthorId)
                .GreaterThan(0).WithMessage("AuthorId must be a positive integer.");
        }
    }
}
