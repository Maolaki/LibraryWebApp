using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryWebApp.AuthorService.Infrastructure.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private ApplicationContext applicationContext = null!;

        public BookRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public IQueryable<Book> GetAll()
        {
            return applicationContext.Books;
        }

        public Book Get(Expression<Func<Book, bool>> predicate)
        {
            var book = applicationContext.Books.FirstOrDefault(predicate);

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
            applicationContext.Entry(book).State = EntityState.Modified;
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
