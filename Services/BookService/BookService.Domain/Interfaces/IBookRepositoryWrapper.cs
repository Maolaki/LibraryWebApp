namespace LibraryWebApp.BookService.Domain.Interfaces
{
    public interface IBookRepositoryWrapper<T> where T : class
    {
        T? GetCacheBookImage(int bookId);
        void SetCacheBookImage(int bookId, T imageDTO);
    }
}
