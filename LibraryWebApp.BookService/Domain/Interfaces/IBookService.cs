using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Services;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Enums;

namespace LibraryWebApp.BookService.Domain.Interfaces
{
    public interface IBookService
    {
        public IEnumerable<Book> GetAllBooks(int pageNumber, int pageSize);
        public IEnumerable<Book> GetAllBooksWithFilters(int pageNumber, int pageSize, string title, BookGenre? genre, int? authorId);
        public Book GetBook(int id);
        public IEnumerable<Book> GetBooksByISBN(string isbn);
        public int GetAvailableCopies(string isbn);
        public void AddBook(Book book);

        public void UpdateBook(Book book);

        public void DeleteBook(Book book);

        public void CheckoutBook(int bookId, string username);
        public void ReturnBook(string username, int id);
        public void AddBookImage(int bookId, ImageDTO imageDto);
        public ImageDTO? GetBookImage(int bookId);
        public void NotifyCheckoutDate(int id);
        byte[] ConvertToByteArray(IFormFile file);
    }
}
