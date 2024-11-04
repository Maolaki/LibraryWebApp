using LibraryWebApp.BookService.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class AddBookImageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBookImageUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(int bookId, IFormFile imageFile)
        {
            var book = _unitOfWork.Books.Get(b => b.Id == bookId);
            if (book == null)
            {
                throw new DirectoryNotFoundException($"Book with ID {bookId} was not found.");
            }

            _unitOfWork.BookRepositoryWrapper.AddBookImage(bookId, imageFile);
            _unitOfWork.Save();
        }
    }
}
