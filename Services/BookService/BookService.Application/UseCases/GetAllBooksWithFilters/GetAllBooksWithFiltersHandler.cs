using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetAllBooksWithFiltersHandler : IRequestHandler<GetAllBooksWithFiltersQuery, IEnumerable<Book>>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public GetAllBooksWithFiltersHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Book>> Handle(GetAllBooksWithFiltersQuery request, CancellationToken cancellationToken)
        {
            var booksQuery = await _unitOfWork.Books.GetAllAsync();

            if (!string.IsNullOrEmpty(request.Title))
            {
                booksQuery = booksQuery
                             .Where(b => b.Title!.Contains(request.Title, StringComparison.OrdinalIgnoreCase))
                             .ToList();
            }

            if (request.AuthorId != null)
            {
                booksQuery = booksQuery
                            .Where(book => book.AuthorId == request.AuthorId)
                            .ToList();
            }

            if (request.Genre.HasValue)
            {
                booksQuery = booksQuery
                            .Where(book => book.Genre == request.Genre.Value)
                            .ToList();
            }

            var uniqueBooks = booksQuery
                .GroupBy(book => book.ISBN)
                .Select(group => group.First())
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            if (!uniqueBooks.Any())
            {
                throw new NotFoundException("Books with filters not found.");
            }

            return await Task.FromResult(uniqueBooks);
        }
    }
}
