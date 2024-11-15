using LibraryWebApp.AuthorService.Application.Exceptions;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAuthorIdHandler : IRequestHandler<GetAuthorIdQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAuthorIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetAuthorIdQuery request, CancellationToken cancellationToken)
        {
            var author = await _unitOfWork.Authors.GetAsync(a => a.FirstName == request.FirstName && a.LastName == request.LastName);

            if (author == null)
                throw new NotFoundException($"Author with name {request.FirstName} {request.LastName} not found.");

            return author.Id;
        }
    }
}