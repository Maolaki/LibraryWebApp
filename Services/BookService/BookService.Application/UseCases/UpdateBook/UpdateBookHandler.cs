using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, Unit>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public UpdateBookHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Books.GetAsync(b => b.Id == request.Id);
            if (existingBook == null)
            {
                throw new NotFoundException($"Book with Id {request.Id} not found.");
            }

            existingBook.ISBN = request.ISBN ?? existingBook.ISBN;
            existingBook.Title = request.Title ?? existingBook.Title;
            existingBook.Description = request.Description ?? existingBook.Description;
            existingBook.Genre = request.Genre ?? existingBook.Genre;
            existingBook.AuthorId = request.AuthorId ?? existingBook.AuthorId;
            existingBook.UserId = request.UserId ?? existingBook.UserId;
            existingBook.CheckoutDateTime = request.CheckoutDateTime ?? existingBook.CheckoutDateTime;
            existingBook.ReturnDateTime = request.ReturnDateTime ?? existingBook.ReturnDateTime;

            _unitOfWork.Books.Update(existingBook);
            await _unitOfWork.SaveAsync();

            return await Task.FromResult(Unit.Value);
        }
    }
}
