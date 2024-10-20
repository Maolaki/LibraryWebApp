using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using LibraryWebApp.AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryWebApp.AuthService.Infrastructure.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private ApplicationContext applicationContext = null!;

        public UserRepository(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public IQueryable<User> GetAll()
        {
            return applicationContext.Users;
        }

        public User? Get(Expression<Func<User, bool>> predicate)
        {
            var user = applicationContext.Users.FirstOrDefault(predicate);

            return user;
        }

        public void Create(User user)
        {
            applicationContext.Users.Add(user);
        }

        public void Update(User user)
        {
            applicationContext.Entry(user).State = EntityState.Modified;
        }

        public void Delete(User user)
        {
            var dbUser = applicationContext.Users.FirstOrDefault(u => u == user);

            if (dbUser == null)
                throw new DirectoryNotFoundException($"User with Id: {user.Id} is not founded.");

            if (dbUser != null)
                applicationContext.Users.Remove(dbUser);

        }


    }
}
