using LibraryWebApp.BookService.Application.Interfaces;
using System.Security.Claims;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class CheckoutBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutBookUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(int bookId, ClaimsPrincipal user)
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

            if (existingBook.UserId != null)
            {
                throw new ArgumentException("Book has already been taken");
            }

            DateTime dateTime = DateTime.Now;
            existingBook.CheckoutDateTime = dateTime;
            existingBook.ReturnDateTime = dateTime.AddDays(14);
            existingBook.UserId = existingUser.Id;
            _unitOfWork.Save();
        }
    }
}
