using LibraryWebApp.BookService.Domain.Entities;

namespace LibraryWebApp.BookService.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetAllWithFiltersAsync(int pageNumber, int pageSize, object? param1, object? param2, object? param3);
    }
}
