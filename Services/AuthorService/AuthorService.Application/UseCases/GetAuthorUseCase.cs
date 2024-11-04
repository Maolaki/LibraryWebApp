using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAuthorUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Author Execute(int id)
        {
            var existingAuthor = _unitOfWork.Authors.Get(a => a.Id == id);
            if (existingAuthor == null)
            {
                throw new DirectoryNotFoundException($"Author with ID {id} not found.");
            }

            return existingAuthor;
        }
    }
}
