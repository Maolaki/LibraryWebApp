using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class DeleteBookCommand : IRequest<Unit>
    {
        public int BookId { get; }

        public DeleteBookCommand(int bookId)
        {
            BookId = bookId;
        }
    }
}