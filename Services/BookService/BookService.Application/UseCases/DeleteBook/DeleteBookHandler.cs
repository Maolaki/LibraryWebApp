using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, Unit>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public DeleteBookHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Books.GetAsync(b => b.Id == request.BookId);
            if (existingBook == null)
            {
                throw new NotFoundException($"Book with Id {request.BookId} not found.");
            }

            _unitOfWork.Books.Delete(existingBook);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}