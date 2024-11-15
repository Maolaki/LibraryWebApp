using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record GetAvailableCopiesQuery(string ISBN) : IRequest<int>;
}
