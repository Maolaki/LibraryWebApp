using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public record GetAuthorIdQuery(string FirstName, string LastName) : IRequest<int>;
}