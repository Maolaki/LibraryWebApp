using AutoMapper;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryWebApp.BookService.Infrastructure.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private ApplicationContext applicationContext = null!;
        private IMapper mapper = null!;

        public BookRepository(ApplicationContext applicationContext, IMapper mapper)
        {
            this.applicationContext = applicationContext;
            this.mapper = mapper;
        }

        public IQueryable<Book> GetAll()
        {
            return applicationContext.Books;
        }

        public Book Get(Expression<Func<Book, bool>> predicate)
        {
            var book = applicationContext.Books
                .Include(b => b.Author)
                .Include(b => b.User)
                .FirstOrDefault(predicate);

            if (book == null)
                throw new DirectoryNotFoundException($"Book with predicate: {predicate} is not founded.");

            return book;
        }

        public void Create(Book book)
        {
            applicationContext.Books.Add(book);
        }

        public void Update(Book book)
        {
            var existingBook = applicationContext.Books.Find(book.Id);

            mapper.Map(book, existingBook);
        }

        public void Delete(Book book)
        {
            var dbBook = applicationContext.Books.FirstOrDefault(t => t == book);

            if (dbBook == null)
                throw new DirectoryNotFoundException($"Book {book} is not founded.");

            if (dbBook != null)
                applicationContext.Books.Remove(dbBook);

        }


    }
}
