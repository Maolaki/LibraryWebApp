using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.AuthorService.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public int GetAuthorId(string firstName, string lastName)
        {
            var author = _unitOfWork.Authors.GetAll()
                    .FirstOrDefault(a => a.FirstName!.Equals(firstName)
                                     && a.LastName!.Equals(lastName));

            return author!.Id;
        }

        public IEnumerable<Author> GetAllAuthors(int pageNumber, int pageSize)
        {
            return _unitOfWork.Authors.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Include(a => a.Books)
                .Take(pageSize);
        }

        public void AddAuthor(Author author)
        {
            _unitOfWork.Authors.Create(author);
            _unitOfWork.Save();
        }

        public Author GetAuthor(int id)
        {
            var author = _unitOfWork.Authors.Get(a => a.Id == id);

            return author;
        }

        public void UpdateAuthor(Author author)
        {
            _unitOfWork.Authors.Update(author);
            _unitOfWork.Save();
        }

        public void DeleteAuthor(Author author)
        {
            _unitOfWork.Authors.Delete(author);
            _unitOfWork.Save();
        }

        public IEnumerable<Book> GetAllBooksByAuthor(Author author, int pageNumber, int pageSize)
        {
            return _unitOfWork.Books.GetAll()
                .Include(b => b.Author)
                .Where(b => b.Author == author)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
