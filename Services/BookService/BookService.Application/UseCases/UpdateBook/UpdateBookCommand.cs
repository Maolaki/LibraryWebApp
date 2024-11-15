using LibraryWebApp.BookService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record UpdateBookCommand(Book UpdatedBook) : IRequest<Unit>;
}
