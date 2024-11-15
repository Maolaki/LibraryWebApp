using LibraryWebApp.BookService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record GetAllBooksQuery(int PageNumber, int PageSize) : IRequest<IEnumerable<Book>>;
}
