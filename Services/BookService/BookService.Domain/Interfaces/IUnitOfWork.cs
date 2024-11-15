using LibraryWebApp.BookService.Domain.Entities;

namespace LibraryWebApp.BookService.Domain.Interfaces
{
    public interface IUnitOfWork<T> where T : class
    {
        IRepository<User> Users { get; }
        IRepository<Author> Authors { get; }
        IRepository<Book> Books { get; }
        IBookRepositoryWrapper<T> BookRepositoryWrapper { get; }
        Task<int> SaveAsync();
    }
}
