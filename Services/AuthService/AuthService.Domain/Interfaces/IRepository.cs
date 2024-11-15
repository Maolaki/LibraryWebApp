using System.Linq.Expressions;

namespace LibraryWebApp.AuthService.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        void Create(T item);
        void Delete(T item);
    }
}
