using LibraryWebApp.BookService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record GetBookByIdQuery(int Id) : IRequest<Book>;
}
