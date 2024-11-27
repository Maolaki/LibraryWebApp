using LibraryWebApp.AuthorService.Domain.Interfaces;
using MediatR;
using LibraryWebApp.AuthorService.Application.Exceptions;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAuthorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var existingAuthor = await _unitOfWork.Authors.GetAsync(a => a.Id == request.Id);

            if (existingAuthor == null)
                throw new NotFoundException($"Author with ID {request.Id} not found.");

            existingAuthor.FirstName = request.FirstName ?? existingAuthor.FirstName;
            existingAuthor.LastName = request.LastName ?? existingAuthor.LastName;
            existingAuthor.DateOfBirth = request.DateOfBirth ?? existingAuthor.DateOfBirth;
            existingAuthor.Country = request.Country ?? existingAuthor.Country;

            _unitOfWork.Authors.Update(existingAuthor);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}