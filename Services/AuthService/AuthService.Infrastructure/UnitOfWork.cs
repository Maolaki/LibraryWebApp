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

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
