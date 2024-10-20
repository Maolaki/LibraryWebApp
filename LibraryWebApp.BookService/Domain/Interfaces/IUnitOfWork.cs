using LibraryWebApp.BookService.Domain.Entities;

namespace LibraryWebApp.BookService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Author> Authors { get; }
        IRepository<Book> Books { get; }
        void Save();
    }
}
