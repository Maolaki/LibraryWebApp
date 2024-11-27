using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class AddAuthorHandler : IRequestHandler<AddAuthorCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddAuthorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var newAuthor = new Author
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Country = request.Country,
                Books = new List<Book>(),
            };

            _unitOfWork.Authors.Create(newAuthor);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
