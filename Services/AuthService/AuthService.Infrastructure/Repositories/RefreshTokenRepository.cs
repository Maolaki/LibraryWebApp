using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Infrastructure.Context;

namespace LibraryWebApp.AuthService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>
    {
        public RefreshTokenRepository(ApplicationContext applicationContext) : base(applicationContext) { }
    }
}
