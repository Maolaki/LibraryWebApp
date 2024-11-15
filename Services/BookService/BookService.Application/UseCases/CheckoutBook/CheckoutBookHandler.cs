using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class CheckoutBookHandler : IRequestHandler<CheckoutBookCommand, Unit>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public CheckoutBookHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CheckoutBookCommand request, CancellationToken cancellationToken)
        {
            var username = request.User.Identity?.Name;

            var existingUser = await _unitOfWork.Users.GetAsync(u => u.Username == username);
            if (existingUser == null)
            {
                throw new NotFoundException($"User with username {username} not found.");
            }

            var existingBook = await _unitOfWork.Books.GetAsync(b => b.Id == request.BookId);
            if (existingBook == null)
            {
                throw new NotFoundException($"Book with Id {request.BookId} not found.");
            }

            if (existingBook.UserId != null)
            {
                throw new ArgumentException("Book has already been taken");
            }

            var dateTime = DateTime.Now;
            existingBook.CheckoutDateTime = dateTime;
            existingBook.ReturnDateTime = dateTime.AddDays(14);
            existingBook.UserId = existingUser.Id;

            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
