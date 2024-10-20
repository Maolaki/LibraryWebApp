using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using LibraryWebApp.AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
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

            if (token == null)
                throw new DirectoryNotFoundException($"Token with predicate: {predicate} is not founded.");

            return token;
        }

        public void Create(RefreshToken refreshToken)
        {
            applicationContext.RefreshTokens.Add(refreshToken);
        }

        public void Update(RefreshToken refreshToken)
        {
            applicationContext.Entry(refreshToken).State = EntityState.Modified;
        }

        public void Delete(RefreshToken refreshToken)
        {
            var token = applicationContext.RefreshTokens.FirstOrDefault(t => t == refreshToken);

            if (token == null)
                throw new DirectoryNotFoundException($"Token {refreshToken} is not founded.");

            if (token != null)
                applicationContext.RefreshTokens.Remove(token);

        }


    }
}
