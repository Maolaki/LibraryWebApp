using AutoMapper;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryWebApp.BookService.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>
    {
        private IMapper mapper = null!;

        public BookRepository(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext)
        {
            this.mapper = mapper;
        }

        public override async Task<Book?> GetAsync(Expression<Func<Book, bool>> predicate)
        {
            return await _dbSet
                .Include(b => b.Author)
                .Include(b => b.User)
                .FirstOrDefaultAsync(predicate);
        }

        public override void Update(Book existingBook, Book book)
        {
            mapper.Map(book, existingBook);
        }
    }
}
