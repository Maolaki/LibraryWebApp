using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using LibraryWebApp.AuthService.Infrastructure.Context;
using LibraryWebApp.AuthService.Infrastructure.Repositories;

namespace LibraryWebApp.AuthService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext _context;
        private IRepository<User>? userRepository;
        private IRepository<RefreshToken>? refreshTokenRepository;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(_context);
                return userRepository;
            }
        }

        public IRepository<RefreshToken> RefreshTokens
        {
            get
            {
                if (refreshTokenRepository == null)
                    refreshTokenRepository = new RefreshTokenRepository(_context);
                return refreshTokenRepository;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
