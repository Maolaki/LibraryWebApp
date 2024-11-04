using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Infrastructure.Context;
using System.Linq.Expressions;

namespace LibraryWebApp.AuthorService.Infrastructure.Repositories
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

        public void Update(User existingUser, User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            applicationContext.Users.Remove(user);
        }


    }
}
