using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAllAuthorsHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<Author>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAuthorsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Author>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var allAuthors = await _unitOfWork.Authors.GetAllAsync();

            var pagedAuthors = allAuthors
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return pagedAuthors;
        }
    }
}
