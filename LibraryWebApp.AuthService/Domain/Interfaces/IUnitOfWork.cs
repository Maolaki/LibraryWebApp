using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Infrastructure.Repositories;

namespace LibraryWebApp.AuthService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        void Save();
    }
}
