using LibraryWebApp.AuthorService.Application.Exceptions;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAllBooksByAuthorHandler : IRequestHandler<GetAllBooksByAuthorQuery, IEnumerable<Book>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBooksByAuthorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Book>> Handle(GetAllBooksByAuthorQuery request, CancellationToken cancellationToken)
        {
            var existingAuthor = await _unitOfWork.Authors.GetAsync(a => a.Id == request.AuthorId);

            if (existingAuthor == null)
                throw new NotFoundException($"Author with ID {request.AuthorId} not found.");

            var allBooks = await _unitOfWork.Books.GetAllAsync(request.PageNumber, request.PageSize);

            var filterdBooks = allBooks
                .Where(b => b.Author == existingAuthor)
                .ToList();

            return filterdBooks;
        }
    }
}