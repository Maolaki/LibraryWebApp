using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Domain.Entities;
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
            var newBook = new Book
            {
                ISBN = request.ISBN,
                Title = request.Title,
                Description = request.Description,
                Genre = request.Genre,
                AuthorId = request.AuthorId,
                UserId = request.UserId,
                CheckoutDateTime = request.CheckoutDateTime,
                ReturnDateTime = request.ReturnDateTime,
            };

            _unitOfWork.Books.Create(newBook);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
