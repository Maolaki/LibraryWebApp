using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryWebApp.AuthorService.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext _applicationContext;
        protected DbSet<T> _dbSet;

        public BaseRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _dbSet = _applicationContext.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T existingEntity, T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
