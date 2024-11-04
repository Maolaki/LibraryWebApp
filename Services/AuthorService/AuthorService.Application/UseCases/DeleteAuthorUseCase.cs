using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class DeleteAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAuthorUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(int authorId)
        {
            var existingAuthor = _unitOfWork.Authors.Get(a => a.Id == authorId);
            if (existingAuthor == null)
            {
                throw new DirectoryNotFoundException($"Author with ID {authorId} not found.");
            }

            _unitOfWork.Authors.Delete(existingAuthor);
            _unitOfWork.Save();
        }
    }
}
