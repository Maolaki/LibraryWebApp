using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Infrastructure.Context;

namespace LibraryWebApp.AuthorService.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
