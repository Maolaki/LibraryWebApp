using LibraryWebApp.AuthorService.Domain.Entities;

namespace LibraryWebApp.AuthorService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Author> Authors { get; }
        IRepository<Book> Books { get; }
        void Save();
    }
}
