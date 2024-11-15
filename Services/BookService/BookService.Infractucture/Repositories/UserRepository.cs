using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Infrastructure.Context;

namespace LibraryWebApp.BookService.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ApplicationContext applicationContext) : base(applicationContext) {}
    }
}
