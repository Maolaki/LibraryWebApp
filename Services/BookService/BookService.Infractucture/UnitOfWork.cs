using AutoMapper;
using LibraryWebApp.BookService.Infrastructure.Repositories;
using LibraryWebApp.BookService.Application.Profiles;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Infrastructure.Context;
using Microsoft.Extensions.Caching.Memory;
using LibraryWebApp.BookService.Application.Interfaces;

namespace LibraryWebApp.BookService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationContext _context;
        private IMapper? mapper;
        private IMemoryCache _memoryCache;
        private IRepository<User>? userRepository;
        private IRepository<Author>? authorRepository;
        private BookRepositoryWrapper? bookRepository;

        public UnitOfWork(ApplicationContext context, IMemoryCache memoryCache)
        {
            _context = context;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>(); 
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

        public IRepository<Book> Books
        {
            get
            {
                if (bookRepository == null)
                    bookRepository = new BookRepositoryWrapper(_context, _memoryCache, mapper!);
                return bookRepository;
            }
        }

        public IBookRepositoryWrapper BookRepositoryWrapper
        {
            get
            {
                if (bookRepository == null)
                    bookRepository = new BookRepositoryWrapper(_context, _memoryCache, mapper!);
                return bookRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
