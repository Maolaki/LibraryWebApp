using LibraryWebApp.AuthorService.Domain.Entities;

namespace LibraryWebApp.AuthorService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Author> Authors { get; }
        IRepository<Book> Books { get; }
        Task<int> SaveAsync();
    }
}
