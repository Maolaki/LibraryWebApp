using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Enums;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetAllBooksWithFiltersUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBooksWithFiltersUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Book> Execute(int pageNumber, int pageSize, string? title, BookGenre? genre, int? authorId = null)
        {
            var booksQuery = _unitOfWork.Books.GetAll();

            if (title != null && string.IsNullOrEmpty(title))
            {
                booksQuery = booksQuery.AsEnumerable()
                             .Where(b => b.Title!.Contains(title, StringComparison.OrdinalIgnoreCase))
                             .AsQueryable();
            }

            if (authorId != null)
            {
                booksQuery = booksQuery.AsEnumerable()
                            .Where(book => book.AuthorId == authorId)
                            .AsQueryable();
            }

            if (genre.HasValue)
            {
                booksQuery = booksQuery.AsEnumerable()
                            .Where(book => book.Genre == genre.Value)
                            .AsQueryable();
            }

            var uniqueBooks = booksQuery
                .GroupBy(book => book.ISBN)
                .Select(group => group.First())
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            if (!uniqueBooks.Any())
            {
                throw new DirectoryNotFoundException($"Books with filters not found.");
            }

            return uniqueBooks;
        }
    }
}
