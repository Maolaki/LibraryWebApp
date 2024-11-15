using LibraryWebApp.AuthorService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public record GetAllBooksByAuthorQuery(int AuthorId, int PageNumber, int PageSize) : IRequest<IEnumerable<Book>>;
}