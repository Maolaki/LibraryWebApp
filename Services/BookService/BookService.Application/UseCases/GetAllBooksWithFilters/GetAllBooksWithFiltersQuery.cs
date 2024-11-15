using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Enums;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record GetAllBooksWithFiltersQuery(
        int PageNumber,
        int PageSize,
        string? Title = null,
        BookGenre? Genre = null,
        int? AuthorId = null) 
        : IRequest<IEnumerable<Book>>;
}
