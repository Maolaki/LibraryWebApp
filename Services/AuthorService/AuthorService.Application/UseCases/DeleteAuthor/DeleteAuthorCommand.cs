using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public record DeleteAuthorCommand(int AuthorId) : IRequest<Unit>;
}
