using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;

namespace LibraryWebApp.BookService.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Author> Authors { get; }
        IRepository<Book> Books { get; }
        IBookRepositoryWrapper BookRepositoryWrapper { get; }
        void Save();
    }
}
