using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using LibraryWebApp.AuthService.Infrastructure.Context;
using System.Linq.Expressions;

namespace LibraryWebApp.AuthService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRepository<RefreshToken>
    {
        private ApplicationContext applicationContext = null!;

        public RefreshTokenRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public IQueryable<RefreshToken> GetAll()
        {
            return applicationContext.RefreshTokens;
        }

        public RefreshToken? Get(Expression<Func<RefreshToken, bool>> predicate)
        {
            var token = applicationContext.RefreshTokens.FirstOrDefault(predicate);

            return token;
        }

        public void Create(RefreshToken refreshToken)
        {
            applicationContext.RefreshTokens.Add(refreshToken);
        }

        public void Update(RefreshToken existingRefreshToken, RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }

        public void Delete(RefreshToken refreshToken)
        {
            applicationContext.RefreshTokens.Remove(refreshToken);
        }


    }
}
