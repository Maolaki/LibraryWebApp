using LibraryWebApp.AuthorService.Application.UseCases;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using Moq;
using FluentAssertions;
using LibraryWebApp.AuthorService.Domain.Enums;

namespace LibraryWebApp.Tests
{
    public class AuthorServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public AuthorServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task AddAuthorHandlerTest()
        {
            // Arrange
            var command = new AddAuthorCommand(
                Id: 1,
                FirstName: "John",
                LastName: "Doe",
                DateOfBirth: new DateOnly(999, 1, 1),
                Country: Country.Argentina);

            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));

            _unitOfWorkMock.Setup(u => u.Authors.Create(It.IsAny<Author>()));

            var handler = new AddAuthorHandler(_unitOfWorkMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(u => u.Authors.Create(It.IsAny<Author>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAuthorHandlerTest()
        {
            // Arrange
            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
            var command = new DeleteAuthorCommand(author.Id);
            _unitOfWorkMock.Setup(u => u.Authors.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Author, bool>>>()))
                .ReturnsAsync(author);
            _unitOfWorkMock.Setup(u => u.Authors.Delete(It.IsAny<Author>()));
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));

            var handler = new DeleteAuthorHandler(_unitOfWorkMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(u => u.Authors.Delete(It.IsAny<Author>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAuthorHandlerTest()
        {
            // Arrange
            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
            var query = new GetAuthorQuery(author.Id);
            _unitOfWorkMock.Setup(u => u.Authors.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Author, bool>>>()))
                .ReturnsAsync(author);

            var handler = new GetAuthorHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(author);
        }

        [Fact]
        public async Task GetAuthorIdHandlerTest()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var author = new Author { Id = 1, FirstName = firstName, LastName = lastName };
            var query = new GetAuthorIdQuery(firstName, lastName);
            _unitOfWorkMock.Setup(u => u.Authors.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Author, bool>>>()))
                .ReturnsAsync(author);

            var handler = new GetAuthorIdHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().Be(author.Id);
        }
    }
}
