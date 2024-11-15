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
            _unitOfWork.Authors.Create(request.Author);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
