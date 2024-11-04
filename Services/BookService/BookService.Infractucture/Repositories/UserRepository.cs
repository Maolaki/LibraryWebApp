using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
using System.Linq.Expressions;

namespace LibraryWebApp.BookService.Infrastructure.Repositories
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

        public void Update(User exisringUser, User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            applicationContext.Users.Remove(user);
        }


    }
}
