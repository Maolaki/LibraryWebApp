using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Infrastructure.Context;

namespace LibraryWebApp.BookService.Infrastructure.Repositories
{
    public class AuthorRepository : BaseRepository<Author>
    {
        public AuthorRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
