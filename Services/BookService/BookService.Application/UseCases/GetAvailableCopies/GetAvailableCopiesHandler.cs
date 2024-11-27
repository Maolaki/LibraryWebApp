using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetAvailableCopiesHandler : IRequestHandler<GetAvailableCopiesQuery, int>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public GetAvailableCopiesHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetAvailableCopiesQuery request, CancellationToken cancellationToken)
        {
            var allBooks = await _unitOfWork.Books.GetAllAsync(1, int.MaxValue);
            
            var filteredBooks = allBooks
                .Where(b => b.ISBN == request.ISBN)
                .ToList();

            if (!filteredBooks.Any())
            {
                throw new NotFoundException($"Book with ISBN {request.ISBN} not found.");
            }

            int totalCopies = filteredBooks.Count;
            int borrowedCopies = filteredBooks.Count(b => b.UserId != null);
            int availableCopies = totalCopies - borrowedCopies;

            return await Task.FromResult(availableCopies);
        }
    }
}
