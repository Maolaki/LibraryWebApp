using LibraryWebApp.BookService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record GetBooksByISBNQuery(string ISBN) : IRequest<IEnumerable<Book>>;
}
