using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAllBooksByAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBooksByAuthorUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Book> Execute(int authorId, int pageNumber, int pageSize)
        {
            var existingAuthor = _unitOfWork.Authors.Get(a => a.Id == authorId);
            if (existingAuthor == null)
            {
                throw new DirectoryNotFoundException($"Author with ID {authorId} not found.");
            }

            return _unitOfWork.Books.GetAll()
                .Include(b => b.Author)
                .Where(b => b.Author == existingAuthor)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
