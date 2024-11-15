using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryWebApp.AuthorService.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>
    {
        public BookRepository(ApplicationContext applicationContext) : base(applicationContext) {}

        public override async Task<Book?> GetAsync(Expression<Func<Book, bool>> predicate)
        {
            return await _dbSet
                .Include(b => b.Author)
                .Include(b => b.User)
                .FirstOrDefaultAsync(predicate);
        }
    }
}
