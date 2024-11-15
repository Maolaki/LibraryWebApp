using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class AddBookHandler : IRequestHandler<AddBookCommand, Unit>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public AddBookHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Books.GetAsync(b => b.Id == request.Book.Id);
            if (existingBook != null)
            {
                throw new ArgumentException($"Book with Id {request.Book.Id} already exists.");
            }

            _unitOfWork.Books.Create(request.Book);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
