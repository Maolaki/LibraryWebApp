using FluentValidation;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class AddBookImageCommandValidator : AbstractValidator<AddBookImageCommand>
    {
        public AddBookImageCommandValidator()
        {
            RuleFor(command => command.BookId)
                .GreaterThan(0)
                .WithMessage("BookId must be a positive integer.");

            RuleFor(command => command.ImageFile)
                .NotNull()
                .WithMessage("ImageFile is required.")
                .Must(file => file.Length <= 5000000)
                .WithMessage("Image size must not exceed 5 MB.")
                .Must(file => file.ContentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase) ||
                              file.ContentType.Equals("image/png", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Image must be of type JPEG or PNG.");
        }
    }
}
