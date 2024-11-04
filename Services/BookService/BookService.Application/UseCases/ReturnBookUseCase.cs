using LibraryWebApp.BookService.Application.Interfaces;
using System.Security.Claims;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class ReturnBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReturnBookUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(ClaimsPrincipal user, int bookId)
        {
            var username = user.Identity!.Name;

            var existingUser = _unitOfWork.Users.Get(u => u.Username == username);
            if (existingUser == null)
            {
                throw new DirectoryNotFoundException($"User with username {username} not found.");
            }

            var existingBook = _unitOfWork.Books.Get(b => b.Id == bookId);
            if (existingBook == null)
            {
                throw new DirectoryNotFoundException($"Book with Id {bookId} not found.");
            }

            if (existingBook.UserId == null)
            {
                throw new ArgumentException("Book is not currently checked out");
            }

            if (existingBook.UserId != null)
            {
                throw new ArgumentException("User has no access");
            }

            existingBook.UserId = null;
            existingBook.CheckoutDateTime = null;
            existingBook.ReturnDateTime = null;

            _unitOfWork.Save();
        }
    }
}
