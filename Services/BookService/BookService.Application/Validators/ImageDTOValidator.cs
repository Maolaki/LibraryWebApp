using FluentValidation;
using LibraryWebApp.BookService.Application.DTOs;

namespace LibraryWebApp.BookService.Application.Validators
{
    public class ImageDTOValidator : AbstractValidator<ImageDTO>
    {
        public ImageDTOValidator()
        {
            RuleFor(imageDto => imageDto.Image)
                .NotNull().WithMessage("Image is required.")
                .Must(image => image!.Length <= 5000000)
                .WithMessage("Image size must not exceed 5 MB.");

            RuleFor(imageDto => imageDto.ImageContentType)
                .NotEmpty().WithMessage("ImageContentType is required.")
                .Must(contentType => contentType!.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase) ||
                                     contentType.Equals("image/png", StringComparison.OrdinalIgnoreCase))
                .WithMessage("ImageContentType must be either 'image/jpeg' or 'image/png'.");
        }
    }
}