using LibraryWebApp.BookService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class AddBookCommand : IRequest<Unit>
    {
        public Book Book { get; }

        public AddBookCommand(Book book)
        {
            Book = book;
        }
    }
}
