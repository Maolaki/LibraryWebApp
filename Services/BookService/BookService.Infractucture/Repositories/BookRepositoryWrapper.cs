using AutoMapper;
using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
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

        public ImageDTO? GetCacheBookImage(int bookId)
        {
            var cacheKey = $"book-image-{bookId}";

            if (_memoryCache.TryGetValue(cacheKey, out ImageDTO? cachedImage))
            {
                return cachedImage!;
            }

            return null;
        }

        public void SetCacheBookImage(int bookId, ImageDTO imageDto)
        {
            var cacheKey = $"book-image-{bookId}";
            _memoryCache.Set(cacheKey, imageDto, TimeSpan.FromDays(1));
        }


    }
}
