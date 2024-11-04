using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Domain.Entities;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetAllBooksUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBooksUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Book> Execute(int pageNumber, int pageSize)
        {
            var uniqueBooks = _unitOfWork.Books.GetAll()
                .GroupBy(book => book.ISBN)
                .Select(group => group.First())
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            if (!uniqueBooks.Any())
            {
                throw new DirectoryNotFoundException($"Books not found.");
            }

            return uniqueBooks;
        }
    }
}
