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
            var existingBook = await _unitOfWork.Books.GetAsync(b => b.Id == request.UpdatedBook.Id);
            if (existingBook == null)
            {
                throw new NotFoundException($"Book with Id {request.UpdatedBook.Id} not found.");
            }

            _unitOfWork.Books.Update(existingBook, request.UpdatedBook);
            await _unitOfWork.SaveAsync();

            return await Task.FromResult(Unit.Value);
        }
    }
}
