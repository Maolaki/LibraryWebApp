using LibraryWebApp.AuthService.Domain.Entities;

namespace LibraryWebApp.AuthService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        void Save();
    }
}
