using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class AddAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddAuthorUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(Author author)
        {
            _unitOfWork.Authors.Create(author);
            _unitOfWork.Save();
        }
    }
}
