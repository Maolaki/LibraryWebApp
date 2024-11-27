using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public record AddBookImageCommand(
        int BookId,
        IFormFile ImageFile
    ) : IRequest<Unit>;
}
