using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Infrastructure.Context;

namespace LibraryWebApp.AuthService.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
