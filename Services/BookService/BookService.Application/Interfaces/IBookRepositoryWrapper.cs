using LibraryWebApp.BookService.Application.DTOs;

namespace LibraryWebApp.BookService.Application.Interfaces
{
    public interface IBookRepositoryWrapper
    {
        ImageDTO? GetCacheBookImage(int bookId);
        void SetCacheBookImage(int bookId, ImageDTO imageDTO);
    }
}
