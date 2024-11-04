using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Infrastructure.Context;
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

        public Book? Get(Expression<Func<Book, bool>> predicate)
        {
            var book = applicationContext.Books.FirstOrDefault(predicate);

            return book;
        }

        public void Create(Book book)
        {
            applicationContext.Books.Add(book);
        }

        public void Update(Book existingBook, Book book)
        {
            throw new NotImplementedException();
        }

        public void Delete(Book book)
        {
            applicationContext.Books.Remove(book);
        }


    }
}
