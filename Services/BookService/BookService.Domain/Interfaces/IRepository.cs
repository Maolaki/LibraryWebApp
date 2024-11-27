using System.Linq.Expressions;

namespace LibraryWebApp.BookService.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}
