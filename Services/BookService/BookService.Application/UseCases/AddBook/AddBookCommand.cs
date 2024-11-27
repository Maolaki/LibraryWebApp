using LibraryWebApp.BookService.Domain.Enums;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record AddBookCommand(
        string? ISBN,
        string? Title,
        string? Description,
        BookGenre Genre,
        int AuthorId,
        long? UserId,
        DateTime? CheckoutDateTime,
        DateTime? ReturnDateTime
    ) : IRequest<Unit>;
}
