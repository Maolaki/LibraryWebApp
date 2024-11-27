using System.Security.Claims;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record CheckoutBookCommand(
        int BookId,
        ClaimsPrincipal User
    ) : IRequest<Unit>;
}
