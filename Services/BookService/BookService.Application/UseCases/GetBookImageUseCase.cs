using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetBookImageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public GetBookImageUseCase(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public ImageDTO Execute(int bookId)
        {
            var book = _unitOfWork.Books.Get(b => b.Id == bookId);
            if (book == null)
            {
                throw new DirectoryNotFoundException($"Book with ID {bookId} was not found.");
            }

            var imageDto = _unitOfWork.BookRepositoryWrapper.GetBookImage(bookId);
            return imageDto;
        }

    }
}
