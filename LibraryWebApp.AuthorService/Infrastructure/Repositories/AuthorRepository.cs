using AutoMapper;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryWebApp.AuthorService.Infrastructure.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private ApplicationContext applicationContext = null!;
        private IMapper mapper;

        public AuthorRepository(ApplicationContext applicationContext, IMapper mapper)
        {
            this.applicationContext = applicationContext;
            this.mapper = mapper;
        }

        public IQueryable<Author> GetAll()
        {
            return applicationContext.Authors;
        }

        public Author Get(Expression<Func<Author, bool>> predicate)
        {
            var author = applicationContext.Authors
                .Include(a => a.Books)
                .FirstOrDefault(predicate);

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
            var existingAuthor = applicationContext.Authors.Find(author.Id);

            mapper.Map(author, existingAuthor);
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
