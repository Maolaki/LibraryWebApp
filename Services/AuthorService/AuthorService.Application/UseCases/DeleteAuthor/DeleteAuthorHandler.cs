using LibraryWebApp.AuthorService.Application.Exceptions;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAuthorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var existingAuthor = await _unitOfWork.Authors.GetAsync(a => a.Id == request.AuthorId);
            if (existingAuthor == null)
            {
                throw new NotFoundException($"Author with ID {request.AuthorId} not found.");
            }

            _unitOfWork.Authors.Delete(existingAuthor);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
