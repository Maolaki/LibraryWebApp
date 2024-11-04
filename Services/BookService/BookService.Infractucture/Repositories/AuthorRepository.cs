using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
using System.Linq.Expressions;

namespace LibraryWebApp.BookService.Infrastructure.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private ApplicationContext applicationContext = null!;

        public AuthorRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }
         
        public IQueryable<Author> GetAll()
        {
            return applicationContext.Authors;
        }

        public Author? Get(Expression<Func<Author, bool>> predicate)
        {
            var author = applicationContext.Authors.FirstOrDefault(predicate);

            return author;
        }

        public void Create(Author author)
        {
            applicationContext.Authors.Add(author);
        }

        public void Update(Author existingAuthor, Author author)
        {
            throw new NotImplementedException();
        }

        public void Delete(Author author)
        {
            applicationContext.Authors.Remove(author);
        }


    }
}
