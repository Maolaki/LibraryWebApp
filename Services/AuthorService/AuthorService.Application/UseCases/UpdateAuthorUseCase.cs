using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class UpdateAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAuthorUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(Author author)
        {
            var existingAuthor = _unitOfWork.Authors.Get(a => a.Id == author.Id);
            if (existingAuthor == null)
            {
                throw new DirectoryNotFoundException($"Author with ID {author.Id} not found.");
            }

            _unitOfWork.Authors.Update(existingAuthor, author);
            _unitOfWork.Save();
        }
    }
}
