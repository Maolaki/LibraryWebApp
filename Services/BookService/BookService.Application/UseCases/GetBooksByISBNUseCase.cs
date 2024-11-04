using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Domain.Entities;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetBooksByISBNUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBooksByISBNUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Book> Execute(string isbn)
        {
            var existingBooks = _unitOfWork.Books.GetAll()
                .Where(b => b.ISBN == isbn)
                .ToList();

            if (!existingBooks.Any())
            {
                throw new DirectoryNotFoundException($"Books with ISBN {isbn} not found.");
            }

            return existingBooks;
        }
    }
}
