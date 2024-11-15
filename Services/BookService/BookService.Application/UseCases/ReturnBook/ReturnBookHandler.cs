using MediatR;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class ReturnBookHandler : IRequestHandler<ReturnBookCommand, Unit>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public ReturnBookHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var username = request.User.Identity!.Name;

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

            if (existingBook.UserId == null)
            {
                throw new ArgumentException("Book is not currently checked out.");
            }

            if (existingBook.UserId != existingUser.Id)
            {
                throw new ArgumentException("User has no access.");
            }

            existingBook.UserId = null;
            existingBook.CheckoutDateTime = null;
            existingBook.ReturnDateTime = null;

            await _unitOfWork.SaveAsync();

            return await Task.FromResult(Unit.Value);
        }
    }
}
