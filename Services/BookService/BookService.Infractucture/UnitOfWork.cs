using AutoMapper;
using LibraryWebApp.BookService.Infrastructure.Repositories;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
using Microsoft.Extensions.Caching.Memory;
using LibraryWebApp.BookService.Application.DTOs;
using BookService.Application.Profiles;

namespace LibraryWebApp.BookService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork<ImageDTO>
    {
        public readonly ApplicationContext _context;
        private IMapper? mapper;
        private IMemoryCache _memoryCache;
        private IRepository<User>? userRepository;
        private IRepository<Author>? authorRepository;
        private IBookRepositoryWrapper<ImageDTO>? bookRepository;

        public UnitOfWork(ApplicationContext context, IMemoryCache memoryCache)
        {
            _context = context;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BookToBookProfile>();
                cfg.AddProfile<BookToBookDTOProfile>();
            });

            mapper = config.CreateMapper();
            _memoryCache = memoryCache;
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(_context);
                return userRepository;
            }
        }

        public IRepository<Author> Authors
        {
            get
            {
                if (authorRepository == null)
                    authorRepository = new AuthorRepository(_context);
                return authorRepository;
            }
        }

        public IBookRepository Books
        {
            get
            {
                if (bookRepository == null)
                    bookRepository = new BookRepositoryWrapper(_context, _memoryCache, mapper!);
                return (IBookRepository)bookRepository;
            }
        }

        public IBookRepositoryWrapper<ImageDTO> BookRepositoryWrapper
        {
            get
            {
                if (bookRepository == null)
                    bookRepository = new BookRepositoryWrapper(_context, _memoryCache, mapper!);
                return bookRepository;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
