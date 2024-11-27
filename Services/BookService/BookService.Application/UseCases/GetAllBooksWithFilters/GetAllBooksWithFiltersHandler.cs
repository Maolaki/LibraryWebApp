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
            var booksQuery = await _unitOfWork.Books.GetAllWithFiltersAsync(request.PageNumber, request.PageSize, request.Title, request.AuthorId, request.Genre);

            if (!booksQuery.Any())
            {
                throw new NotFoundException("Books with filters not found.");
            }

            return await Task.FromResult(booksQuery);
        }
    }
}
