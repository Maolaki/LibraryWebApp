using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record DeleteBookCommand(int BookId) : IRequest<Unit>;
}
