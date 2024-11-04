using AutoMapper;
using LibraryWebApp.AuthorService.Application.Profiles;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Infrastructure.Context;
using LibraryWebApp.AuthorService.Infrastructure.Repositories;

namespace LibraryWebApp.AuthorService.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext _context;
        private IMapper? mapper;
        private IRepository<User>? userRepository;
        private IRepository<Author>? authorRepository;
        private IRepository<Book>? bookRepository;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>(); 
            });

            mapper = config.CreateMapper();
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
                    authorRepository = new AuthorRepository(_context, mapper!);
                return authorRepository;
            }
        }

        public IRepository<Book> Books
        {
            get
            {
                if (bookRepository == null)
                    bookRepository = new BookRepository(_context);
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
