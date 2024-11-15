using AutoMapper;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Enums;
using LibraryWebApp.AuthorService.Infrastructure.Context;
using LibraryWebApp.AuthorService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.UnitTests
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
            _mapper = new Mapper(new MapperConfiguration(cfg => { }));
            _repository = new AuthorRepository(_context, _mapper);
        }

        [Fact]
        public void Delete_ExistingAuthor_RemovesAuthorFromContext()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.China
            };

            var allAuthors = _context.Authors.ToList();
            if (allAuthors.Any())
                _context.Authors.RemoveRange(allAuthors);
            _context.Authors.Add(author);
            _context.SaveChanges();

            // Act
            _repository.Delete(author);
            _context.SaveChanges();

            // Assert
            Assert.Empty(_context.Authors);
        }

        [Fact]
        public async Task Get_ExistingAuthor_ReturnsAuthorAsync()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.China
            };

            _context.Authors.Add(author);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetAsync(a => a.Id == author.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(author.FirstName, result.FirstName);
        }

        [Fact]
        public async Task Get_NonExistingAuthor_ReturnsNullAsync()
        {
            // Act
            var result = await _repository.GetAsync(a => a.Id == -1);

            // Assert
            Assert.Null(result);
        }
    }
}
