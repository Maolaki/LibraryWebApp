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
            var existingAuthor = await _unitOfWork.Authors.GetAsync(a => a.Id == request.Author.Id);

            if (existingAuthor == null)
                throw new NotFoundException($"Author with ID {request.Author.Id} not found.");

            _unitOfWork.Authors.Update(existingAuthor, request.Author);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}