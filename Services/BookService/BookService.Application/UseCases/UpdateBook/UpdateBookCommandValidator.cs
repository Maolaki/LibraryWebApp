using FluentValidation;
using LibraryWebApp.BookService.Application.UseCases;

namespace LibraryWebApp.BookService.Application.Validators
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(command => command.Id)
                .GreaterThan(0)
                .WithMessage("Id must be a positive integer.");

            RuleFor(command => command.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Length(10, 17).WithMessage("ISBN must be between 10 and 17 characters.")
                .When(command => command.ISBN != null);

            RuleFor(command => command.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.")
                .When(command => command.Title != null);

            RuleFor(command => command.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
                .When(command => command.Description != null);

            RuleFor(command => command.Genre)
                .IsInEnum().WithMessage("Genre must be a valid value.");

            RuleFor(command => command.AuthorId)
                .GreaterThan(0).WithMessage("AuthorId must be a positive integer.");

            RuleFor(command => command.CheckoutDateTime)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CheckoutDateTime cannot be in the future.")
                .When(command => command.CheckoutDateTime.HasValue);

            RuleFor(command => command.ReturnDateTime)
                .GreaterThan(command => command.CheckoutDateTime.GetValueOrDefault())
                .WithMessage("ReturnDateTime must be after CheckoutDateTime.")
                .When(command => command.ReturnDateTime.HasValue && command.CheckoutDateTime.HasValue);
        }
    }
}
