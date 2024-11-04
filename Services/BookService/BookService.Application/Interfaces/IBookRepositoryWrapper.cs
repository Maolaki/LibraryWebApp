using LibraryWebApp.BookService.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace LibraryWebApp.BookService.Domain.Interfaces
{
    public interface IBookRepositoryWrapper
    {
        ImageDTO GetBookImage(int bookId);
        void AddBookImage(int bookId, IFormFile imageFile);
    }
}
