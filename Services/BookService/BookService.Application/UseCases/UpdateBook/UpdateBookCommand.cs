using LibraryWebApp.BookService.Domain.Enums;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record UpdateBookCommand(
        int Id,
        string? ISBN,
        string? Title,
        string? Description,
        BookGenre? Genre,
        int? AuthorId,
        long? UserId,
        DateTime? CheckoutDateTime,
        DateTime? ReturnDateTime
    ) : IRequest<Unit>;
}
