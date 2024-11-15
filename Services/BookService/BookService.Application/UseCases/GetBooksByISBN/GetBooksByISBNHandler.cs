using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetBooksByISBNHandler : IRequestHandler<GetBooksByISBNQuery, IEnumerable<Book>>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public GetBooksByISBNHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Book>> Handle(GetBooksByISBNQuery request, CancellationToken cancellationToken)
        {
            var existingBooks = await _unitOfWork.Books.GetAllAsync();

            var filteredBooks = existingBooks
                .Where(b => b.ISBN == request.ISBN)
                .ToList();

            if (!filteredBooks.Any())
            {
                throw new NotFoundException($"Books with ISBN {request.ISBN} not found.");
            }

            return await Task.FromResult(filteredBooks);
        }
    }
}
