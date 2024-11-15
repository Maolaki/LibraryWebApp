using LibraryWebApp.AuthorService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public record UpdateAuthorCommand(Author Author) : IRequest<Unit>;
}