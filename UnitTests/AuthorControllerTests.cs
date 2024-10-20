using AutoMapper;
using LibraryWebApp.AuthorService.Application.DTOs;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Enums;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using LibraryWebApp.AuthorService.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests
{
    public class AuthorsControllerTests
    {
        private readonly Mock<IAuthorService> _mockAuthorService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AuthorsController _controller;

        public AuthorsControllerTests()
        {
            _mockAuthorService = new Mock<IAuthorService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new AuthorsController(_mockAuthorService.Object, _mockMapper.Object);
        }

        [Fact]
        public void AddAuthor_ValidAuthorDTO_ReturnsOkResult()
        {
            var authorDto = new AuthorDTO
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.Algeria
            };
            var author = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.Algeria
            };

            _mockMapper.Setup(m => m.Map<Author>(authorDto)).Returns(author);

            var result = _controller.AddAuthor(authorDto);

            var okResult = Assert.IsType<OkResult>(result);
            _mockAuthorService.Verify(service => service.AddAuthor(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public void UpdateAuthor_ValidAuthorDTO_ReturnsOkResult()
        {
            var authorDto = new AuthorDTO
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.Argentina
            };
            var author = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.Canada
            };

            _mockMapper.Setup(m => m.Map<Author>(authorDto)).Returns(author);

            var result = _controller.UpdateAuthor(1, authorDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Author updated successfully.", okResult.Value);
            _mockAuthorService.Verify(service => service.UpdateAuthor(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public void GetAllAuthors_ReturnsOkResult_WithAuthorsList()
        {
            var authorsList = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateOnly(1980, 5, 15),
                    Country = Country.China
                }
            };

            _mockAuthorService.Setup(service => service.GetAllAuthors(It.IsAny<int>(), It.IsAny<int>())).Returns(authorsList);

            var result = _controller.GetAllAuthors();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Author>>(okResult.Value);
            Assert.Equal(authorsList, returnValue);
        }

        [Fact]
        public void DeleteAuthor_AuthorExists_ReturnsOkResult()
        {
            var author = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.China
            };
            _mockAuthorService.Setup(service => service.GetAuthor(1)).Returns(author);

            var result = _controller.DeleteAuthor(1);

            var okResult = Assert.IsType<OkResult>(result);
            _mockAuthorService.Verify(service => service.DeleteAuthor(It.IsAny<Author>()), Times.Once);
        }

        [Fact]
        public void DeleteAuthor_AuthorDoesNotExist_ReturnsNotFound()
        {
            _mockAuthorService.Setup(service => service.GetAuthor(1)).Returns((Author)null);

            var result = _controller.DeleteAuthor(1);

            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public void GetAllBooksByAuthor_AuthorExists_ReturnsOkResult_WithBooks()
        {
            var author = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.China
            };
            var booksList = new List<Book> { new Book 
            {                 
                Id = 1,
                ISBN = "978-3-16-148410-0",
                Title = "The Great Book",
                Description = "A fascinating book about great things.",
                Genre = BookGenre.Fiction,
                AuthorId = 1,
                UserId = 2,
                CheckoutDateTime = DateTime.Now.AddDays(-2),
                ReturnDateTime = DateTime.Now.AddDays(7) 
            }};
            _mockAuthorService.Setup(service => service.GetAuthor(1)).Returns(author);
            _mockAuthorService.Setup(service => service.GetAllBooksByAuthor(author, It.IsAny<int>(), It.IsAny<int>())).Returns(booksList);

            var result = _controller.GetAllBooksByAuthor(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Book>>(okResult.Value);
            Assert.Equal(booksList, returnValue);
        }

        [Fact]
        public void GetAllBooksByAuthor_AuthorDoesNotExist_ReturnsNotFound()
        {
            _mockAuthorService.Setup(service => service.GetAuthor(1)).Returns((Author)null);

            var result = _controller.GetAllBooksByAuthor(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

    }
}
