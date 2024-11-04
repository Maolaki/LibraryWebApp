using AutoMapper;
using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryWebApp.BookService.Infrastructure.Repositories
{
    public class BookRepositoryWrapper : BookRepository, IBookRepositoryWrapper
    {
        private readonly IMemoryCache _memoryCache;

        public BookRepositoryWrapper(ApplicationContext applicationContext, IMemoryCache memoryCache, IMapper mapper)
            : base(applicationContext, mapper)
        {
            _memoryCache = memoryCache;
        }

        public ImageDTO GetBookImage(int bookId)
        {
            var cacheKey = $"book-image-{bookId}";

            if (_memoryCache.TryGetValue(cacheKey, out ImageDTO? cachedImage))
            {
                return cachedImage!;
            }

            var book = Get(b => b.Id == bookId);
            ValidateBookImage(book, bookId);

            var imageDto = new ImageDTO
            {
                Image = book!.Image,
                ImageContentType = book!.ImageContentType
            };

            _memoryCache.Set(cacheKey, imageDto, TimeSpan.FromDays(1));
            return imageDto;
        }

        public void AddBookImage(int bookId, IFormFile imageFile)
        {
            var existingBook = Get(b => b.Id == bookId);

            var imageDto = ConvertToImageDto(imageFile);
            existingBook!.Image = imageDto.Image;
            existingBook!.ImageContentType = imageDto.ImageContentType;

            var cacheKey = $"book-image-{bookId}";
            _memoryCache.Set(cacheKey, imageDto, TimeSpan.FromDays(1));
        }

        private void ValidateBookImage(Book? book, int bookId)
        {
            if (book == null || book.Image == null || string.IsNullOrEmpty(book.ImageContentType))
            {
                throw new DirectoryNotFoundException($"Image for book with ID {bookId} was not found.");
            }
        }

        private ImageDTO ConvertToImageDto(IFormFile file)
        {
            var imageDto = new ImageDTO
            {
                Image = ConvertToByteArray(file),
                ImageContentType = file.ContentType
            };

            if (imageDto.Image == null || imageDto.ImageContentType == null)
            {
                throw new FormatException("Failed to convert image file to ImageDTO.");
            }

            return imageDto;
        }

        private byte[] ConvertToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
