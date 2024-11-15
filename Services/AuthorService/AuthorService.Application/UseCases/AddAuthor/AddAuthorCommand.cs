using LibraryWebApp.AuthorService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public record AddAuthorCommand(Author Author) : IRequest<Unit>;
}
