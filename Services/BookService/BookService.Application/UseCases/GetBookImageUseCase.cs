using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Domain.Entities;
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
            var existingBook = _unitOfWork.Books.Get(b => b.Id == bookId);
            if (existingBook == null)
            {
                throw new DirectoryNotFoundException($"Book with ID {bookId} was not found.");
            }

            var imageDto = _unitOfWork.BookRepositoryWrapper.GetCacheBookImage(bookId);

            if (imageDto == null)
            {
                ValidateBookImage(existingBook, bookId);

                imageDto = new ImageDTO
                {
                    Image = existingBook.Image,
                    ImageContentType = existingBook.ImageContentType
                };

                _unitOfWork.BookRepositoryWrapper.SetCacheBookImage(bookId, imageDto);
            }
            return imageDto;
        }

        private void ValidateBookImage(Book? book, int bookId)
        {
            if (book == null || book.Image == null || string.IsNullOrEmpty(book.ImageContentType))
            {
                throw new DirectoryNotFoundException($"Image for book with ID {bookId} was not found.");
            }
        }
    }
}
