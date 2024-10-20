using AutoMapper;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Enums;
using LibraryWebApp.AuthorService.Infrastructure.Context;
using LibraryWebApp.AuthorService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public class AuthorRepositoryTests
    {
        private readonly ApplicationContext _context;
        private readonly AuthorRepository _repository;
        private readonly IMapper _mapper;

        public AuthorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationContext(options);
            _mapper = new Mapper(new MapperConfiguration(cfg => {}));
            _repository = new AuthorRepository(_context, _mapper);
        }

        [Fact]
        public void Delete_ExistingAuthor_RemovesAuthorFromContext()
        {
            var author = new Author
            {
                Id = 3,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.China
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            _repository.Delete(author);
            _context.SaveChanges();

            Assert.Empty(_context.Authors);
        }

        [Fact]
        public void Create_ValidAuthor_AddsAuthorToContext()
        {
            var author = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.China
            };

            _repository.Create(author);
            _context.SaveChanges(); 

            var authorsInDb = _context.Authors.ToList();
            Assert.Single(authorsInDb);
            Assert.Equal(author.FirstName, authorsInDb[0].FirstName);
        }

        [Fact]
        public void Delete_NonExistingAuthor_ThrowsDirectoryNotFoundException()
        {
            var nonExistingAuthor = new Author
            {
                Id = -999,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.China
            };

            Assert.Throws<DirectoryNotFoundException>(() => _repository.Delete(nonExistingAuthor));
        }
    }


}