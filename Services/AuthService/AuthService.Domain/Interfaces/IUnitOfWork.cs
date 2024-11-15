using LibraryWebApp.AuthService.Domain.Entities;

namespace LibraryWebApp.AuthService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        Task<int> SaveAsync();
    }
}
