using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class AddBookImageCommand : IRequest<Unit>
    {
        public int BookId { get; }
        public IFormFile ImageFile { get; }

        public AddBookImageCommand(int bookId, IFormFile imageFile)
        {
            BookId = bookId;
            ImageFile = imageFile;
        }
    }
}
