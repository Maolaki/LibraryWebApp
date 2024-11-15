using LibraryWebApp.AuthorService.Application.UseCases;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using MediatR;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

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
        public async Task AddAuthorHandler_ShouldAddAuthor_WhenValid()
        {
            // Arrange
            var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
            var command = new AddAuthorCommand(author);

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
        public async Task DeleteAuthorHandler_ShouldDeleteAuthor_WhenExists()
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
        public async Task GetAllAuthorsHandler_ShouldReturnPagedAuthors_WhenValidRequest()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = 1, FirstName = "John", LastName = "Doe" },
                new Author { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };
            _unitOfWorkMock.Setup(u => u.Authors.GetAllAsync()).ReturnsAsync(authors);
            var query = new GetAllAuthorsQuery(1, 2);

            var handler = new GetAllAuthorsHandler(_unitOfWorkMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(a => a.FirstName == "John");
            result.Should().Contain(a => a.FirstName == "Jane");
        }

        [Fact]
        public async Task GetAuthorHandler_ShouldReturnAuthor_WhenExists()
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
        public async Task GetAuthorIdHandler_ShouldReturnId_WhenAuthorExistsByName()
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

        [Fact]
        public async Task UpdateAuthorHandler_ShouldUpdateAuthor_WhenExists()
        {
            // Arrange
            var existingAuthor = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
            var updatedAuthor = new Author { Id = 1, FirstName = "John", LastName = "Smith" };
            var command = new UpdateAuthorCommand(updatedAuthor);
            _unitOfWorkMock.Setup(u => u.Authors.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Author, bool>>>()))
                .ReturnsAsync(existingAuthor);
            _unitOfWorkMock.Setup(u => u.Authors.Update(It.IsAny<Author>(), It.IsAny<Author>()));
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));

            var handler = new UpdateAuthorHandler(_unitOfWorkMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(u => u.Authors.Update(It.IsAny<Author>(), It.IsAny<Author>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }
    }
}
