using LibraryWebApp.AuthorService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public record GetAllAuthorsQuery(int PageNumber, int PageSize) : IRequest<IEnumerable<Author>>;
}
