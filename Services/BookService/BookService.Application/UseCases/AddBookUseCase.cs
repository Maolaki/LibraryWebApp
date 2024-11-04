using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Domain.Entities;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class AddBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBookUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(Book book)
        {
            var existingBook = _unitOfWork.Books.Get(b => b.Id == book.Id);
            if (existingBook != null)
            {
                throw new DirectoryNotFoundException($"Book with Id: {book.Id} already exist.");
            }

            _unitOfWork.Books.Create(book);
            _unitOfWork.Save();
        }
    }
}
