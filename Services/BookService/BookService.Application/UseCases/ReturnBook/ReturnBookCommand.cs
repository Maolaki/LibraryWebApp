using System.Security.Claims;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record ReturnBookCommand(ClaimsPrincipal User, int BookId) : IRequest<Unit>;
}
