using System.Security.Claims;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class CheckoutBookCommand : IRequest<Unit>
    {
        public int BookId { get; }
        public ClaimsPrincipal User { get; }

        public CheckoutBookCommand(int bookId, ClaimsPrincipal user)
        {
            BookId = bookId;
            User = user;
        }
    }
}
