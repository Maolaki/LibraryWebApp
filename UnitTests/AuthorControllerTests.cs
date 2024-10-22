using AutoMapper;
using LibraryWebApp.AuthorService.Application.DTOs;
using LibraryWebApp.AuthorService.Application.Services;
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
        private readonly AuthorController _controller;

        public AuthorsControllerTests()
        {
            _mockAuthorService = new Mock<IAuthorService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new AuthorController(_mockAuthorService.Object, _mockMapper.Object);
        }

        [Fact]
        public void AddAuthor_ValidAuthorDTO_ReturnsOkResult_AndCallsServiceMethod()
        {
            var authorDto = new AuthorDTO
            {
                FirstName = "AWA",
                LastName = "WAWA",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.Algeria
            };
            var author = new Author
            {
                Id = 1,
                FirstName = "AWA",
                LastName = "WAWA",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.Algeria
            };

            _mockMapper.Setup(m => m.Map<Author>(authorDto)).Returns(author);

            var result = _controller.AddAuthor(authorDto);

            var okResult = Assert.IsType<OkResult>(result);
            _mockAuthorService.Verify(service => service.AddAuthor(author), Times.Once);
        }

        [Fact]
        public void UpdateAuthor_ValidAuthorDTO_ReturnsOkResult_AndCallsServiceMethod()
        {
            var authorDto = new AuthorDTO
            {
                FirstName = "Qer",
                LastName = "Der",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.Argentina
            };
            var author = new Author
            {
                Id = 1,
                FirstName = "Qer",
                LastName = "Der",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.Argentina
            };

            _mockMapper.Setup(m => m.Map<Author>(authorDto)).Returns(author);

            var result = _controller.UpdateAuthor(1, authorDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Author updated successfully.", okResult.Value);
            _mockAuthorService.Verify(service => service.UpdateAuthor(author), Times.Once);
        }

        [Fact]
        public void GetAllAuthors_ReturnsOkResult_WithAuthorDTOList_AndCallsServiceMethod()
        {
            var authorsList = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    FirstName = "Jo",
                    LastName = "Jo",
                    DateOfBirth = new DateOnly(1970, 1, 1),
                    Country = Country.Algeria,
                    Books = new List<Book>()
                }
            };

                    var authorDtos = new List<AuthorDTO>
            {
                new AuthorDTO
                {
                    Id = 1,
                    FirstName = "Jo",
                    LastName = "Jo",
                    Country = Country.Australia,
                }
            };

            _mockAuthorService.Setup(service => service.GetAllAuthors(It.IsAny<int>(), It.IsAny<int>())).Returns(authorsList);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<AuthorDTO>>(authorsList)).Returns(authorDtos);

            var result = _controller.GetAllAuthors(1, 10);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<AuthorDTO>>(okResult.Value);
            Assert.Equal(authorDtos, returnValue);
            _mockAuthorService.Verify(service => service.GetAllAuthors(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<IEnumerable<AuthorDTO>>(authorsList), Times.Once);
        }

        [Fact]
        public void GetAuthorId_ReturnsAuthorId_WhenAuthorExists()
        {
            var firstName = "Jo";
            var lastName = "Jo";
            var author = new Author
            {
                Id = 1,
                FirstName = firstName,
                LastName = lastName
            };

            var authors = new List<Author> { author }.AsQueryable();

            var mockAuthorRepository = new Mock<IRepository<Author>>();
            mockAuthorRepository.Setup(repo => repo.GetAll()).Returns(authors);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.Authors).Returns(mockAuthorRepository.Object);

            var authorService = new AuthorService(mockUnitOfWork.Object);

            var result = authorService.GetAuthorId(firstName, lastName);

            Assert.Equal(author.Id, result);
            mockAuthorRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void DeleteAuthor_AuthorExists_ReturnsOkResult_AndCallsServiceMethod()
        {
            var author = new Author
            {
                Id = 1,
                FirstName = "Jo",
                LastName = "Jo",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.China
            };
            _mockAuthorService.Setup(service => service.GetAuthor(1)).Returns(author);

            var result = _controller.DeleteAuthor(1);

            var okResult = Assert.IsType<OkResult>(result);
            _mockAuthorService.Verify(service => service.DeleteAuthor(author), Times.Once);
        }

        [Fact]
        public void DeleteAuthor_AuthorDoesNotExist_ReturnsNotFound_AndDoesNotCallDelete()
        {
            _mockAuthorService.Setup(service => service.GetAuthor(1)).Returns((Author)null);

            var result = _controller.DeleteAuthor(1);

            Assert.IsType<NotFoundResult>(result);
            _mockAuthorService.Verify(service => service.DeleteAuthor(It.IsAny<Author>()), Times.Never);
        }

        [Fact]
        public void GetAllBooksByAuthor_AuthorExists_ReturnsOkResult_WithBooks_AndCallsServiceMethods()
        {
            var author = new Author
            {
                Id = 1,
                FirstName = "Jo",
                LastName = "Jo",
                DateOfBirth = new DateOnly(1980, 5, 15),
                Country = Country.China
            };
            var booksList = new List<Book> { new Book
            {
                Id = 1,
                ISBN = "978-3-16-148410-0",
                Title = "The Great Book",
                Description = "Cool book",
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
            _mockAuthorService.Verify(service => service.GetAllBooksByAuthor(author, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetAllBooksByAuthor_AuthorDoesNotExist_ReturnsNotFound_AndDoesNotCallBooksService()
        {
            _mockAuthorService.Setup(service => service.GetAuthor(1)).Returns((Author)null);

            var result = _controller.GetAllBooksByAuthor(1);

            Assert.IsType<NotFoundResult>(result.Result);
            _mockAuthorService.Verify(service => service.GetAllBooksByAuthor(It.IsAny<Author>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
