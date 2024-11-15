using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record NotifyCheckoutDateCommand(int BookId) : IRequest<Unit>;
}
