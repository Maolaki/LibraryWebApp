using AutoMapper;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
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

        public Author Get(Expression<Func<Author, bool>> predicate)
        {
            var author = applicationContext.Authors.FirstOrDefault(predicate);

            if (author == null)
                throw new DirectoryNotFoundException($"Author with predicate: {predicate} is not founded.");

            return author;
        }

        public void Create(Author author)
        {
            applicationContext.Authors.Add(author);
        }

        public void Update(Author author)
        {
            applicationContext.Entry(author).State = EntityState.Modified;
        }

        public void Delete(Author author)
        {
            var dbAuthor = applicationContext.Authors.FirstOrDefault(t => t == author);

            if (dbAuthor == null)
                throw new DirectoryNotFoundException($"Author {author} is not founded.");

            if (dbAuthor != null)
                applicationContext.Authors.Remove(dbAuthor);

        }


    }
}
