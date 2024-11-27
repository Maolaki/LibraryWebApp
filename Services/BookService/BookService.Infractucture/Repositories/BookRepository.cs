using AutoMapper;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Enums;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryWebApp.BookService.Infrastructure.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private IMapper mapper = null!;

        public BookRepository(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext)
        {
            this.mapper = mapper;
        }

        public async override Task<IEnumerable<Book>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _dbSet
                .GroupBy(book => book.ISBN)
                .Select(group => group.First()) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllWithFiltersAsync(
            int pageNumber,
            int pageSize,
            object? title,
            object? authorId,
            object? genre)
        {
            IQueryable<Book> booksQuery = _dbSet;

            if (title is string titleString && !string.IsNullOrEmpty(titleString))
            {
                booksQuery = booksQuery.Where(b => b.Title!.Contains(titleString, StringComparison.OrdinalIgnoreCase));
            }

            if (authorId is int authorGuid && authorGuid != 0)
            {
                booksQuery = booksQuery.Where(book => book.AuthorId == authorGuid);
            }

            if (genre is BookGenre genreEnum)
            {
                booksQuery = booksQuery.Where(book => book.Genre == genreEnum);
            }

            return await booksQuery
                .GroupBy(book => book.ISBN)
                .Select(group => group.First())
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<Book?> GetAsync(Expression<Func<Book, bool>> predicate)
        {
            return await _dbSet
                .Include(b => b.Author)
                .Include(b => b.User)
                .FirstOrDefaultAsync(predicate);
        }
    }
}
