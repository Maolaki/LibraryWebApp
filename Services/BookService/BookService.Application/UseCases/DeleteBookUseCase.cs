using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Domain.Entities;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class DeleteBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBookUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(Book book)
        {
            var existingBook = _unitOfWork.Books.Get(b => b.Id == book.Id);
            if (existingBook == null)
            {
                throw new DirectoryNotFoundException($"Book with Id {book.Id} not found.");
            }

            _unitOfWork.Books.Delete(existingBook);
            _unitOfWork.Save();
        }
    }
}
