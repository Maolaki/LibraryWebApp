using LibraryWebApp.AuthorService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public record GetAuthorQuery(int Id) : IRequest<Author>;
}