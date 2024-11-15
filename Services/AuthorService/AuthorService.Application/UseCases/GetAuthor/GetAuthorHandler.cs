using LibraryWebApp.AuthorService.Application.Exceptions;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAuthorHandler : IRequestHandler<GetAuthorQuery, Author>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAuthorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Author> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetAsync(a => a.Id == request.Id);

            if (author == null)
                throw new NotFoundException($"Author with ID {request.Id} not found.");

            return author;
        }
    }
}