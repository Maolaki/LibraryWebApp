using LibraryWebApp.AuthorService.Application.UseCases;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace LibraryWebApp.UnitTests
{
    public class UseCasesTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Author>> _authorRepositoryMock;
        private readonly Author _testAuthor;

        public UseCasesTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _authorRepositoryMock = new Mock<IRepository<Author>>();
            _testAuthor = new Author { Id = 1, FirstName = "John", LastName = "Doe" };

            _unitOfWorkMock.Setup(u => u.Authors).Returns(_authorRepositoryMock.Object);
        }

        [Fact]
        public void AddAuthor_ShouldAddAuthor()
        {
            // Arrange
            var useCase = new AddAuthorUseCase(_unitOfWorkMock.Object);

            // Act
            useCase.Execute(_testAuthor);

            // Assert
            _authorRepositoryMock.Verify(a => a.Create(It.IsAny<Author>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void UpdateAuthor_ShouldUpdateExistingAuthor()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns(_testAuthor);
            var useCase = new UpdateAuthorUseCase(_unitOfWorkMock.Object);
            var updatedAuthor = new Author { Id = 1, FirstName = "Jane", LastName = "Doe" };

            // Act
            useCase.Execute(updatedAuthor);

            // Assert
            _unitOfWorkMock.Verify(u => u.Authors.Update(It.IsAny<Author>(), It.IsAny<Author>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void UpdateAuthor_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns((Author)null);
            var useCase = new UpdateAuthorUseCase(_unitOfWorkMock.Object);
            var updatedAuthor = new Author { Id = 999, FirstName = "Jane", LastName = "Doe" };

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => useCase.Execute(updatedAuthor));
        }

        [Fact]
        public void GetAuthor_ShouldReturnExistingAuthor()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns(_testAuthor);
            var useCase = new GetAuthorUseCase(_unitOfWorkMock.Object);

            // Act
            var result = useCase.Execute(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_testAuthor.Id, result.Id);
        }

        [Fact]
        public void GetAuthor_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns((Author)null);
            var useCase = new GetAuthorUseCase(_unitOfWorkMock.Object);

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => useCase.Execute(999));
        }

        [Fact]
        public void GetAuthorId_ShouldReturnExistingAuthorId()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns(_testAuthor);
            var useCase = new GetAuthorIdUseCase(_unitOfWorkMock.Object);

            // Act
            var result = useCase.Execute("John", "Doe");

            // Assert
            Assert.Equal(_testAuthor.Id, result);
        }

        [Fact]
        public void GetAuthorId_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns((Author)null);
            var useCase = new GetAuthorIdUseCase(_unitOfWorkMock.Object);

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => useCase.Execute("Unknown", "Author"));
        }

        [Fact]
        public void GetAllAuthors_ShouldReturnPagedAuthors()
        {
            // Arrange
            var authors = new List<Author> { _testAuthor };
            _unitOfWorkMock.Setup(u => u.Authors.GetAll())
                .Returns(authors.AsQueryable());
            var useCase = new GetAllAuthorsUseCase(_unitOfWorkMock.Object);

            // Act
            var result = useCase.Execute(1, 10).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal(_testAuthor.Id, result[0].Id);
        }

        [Fact]
        public void DeleteAuthor_ShouldRemoveExistingAuthor()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns(_testAuthor);
            var useCase = new DeleteAuthorUseCase(_unitOfWorkMock.Object);

            // Act
            useCase.Execute(_testAuthor.Id);

            // Assert
            _unitOfWorkMock.Verify(u => u.Authors.Delete(It.IsAny<Author>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Save(), Times.Once);
        }

        [Fact]
        public void DeleteAuthor_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns((Author)null);
            var useCase = new DeleteAuthorUseCase(_unitOfWorkMock.Object);

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => useCase.Execute(999));
        }

        [Fact]
        public void GetAllBooksByAuthor_ShouldReturnBooks_WhenAuthorExists()
        {
            // Arrange
            var book1 = new Book { Id = 1, Title = "Book 1", Author = _testAuthor };
            var book2 = new Book { Id = 2, Title = "Book 2", Author = _testAuthor };
            var books = new List<Book> { book1, book2 };

            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns(_testAuthor);
            _unitOfWorkMock.Setup(u => u.Books.GetAll())
                .Returns(books.AsQueryable());
            var useCase = new GetAllBooksByAuthorUseCase(_unitOfWorkMock.Object);

            // Act
            var result = useCase.Execute(_testAuthor.Id, 1, 10).ToList();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetAllBooksByAuthor_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Authors.Get(It.IsAny<Expression<Func<Author, bool>>>()))
                .Returns((Author)null);
            var useCase = new GetAllBooksByAuthorUseCase(_unitOfWorkMock.Object);

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => useCase.Execute(999, 1, 10));
        }
    }
}
