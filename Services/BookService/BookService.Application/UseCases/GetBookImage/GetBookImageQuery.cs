using LibraryWebApp.BookService.Application.DTOs;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record GetBookImageQuery(int BookId) : IRequest<ImageDTO>;
}
