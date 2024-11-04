using LibraryWebApp.AuthorService.Domain.Interfaces;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAuthorIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAuthorIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Execute(string firstName, string lastName)
        {
            var existingAuthor = _unitOfWork.Authors.Get(a => a.FirstName!.Equals(firstName) && a.LastName!.Equals(lastName));
            if (existingAuthor == null)
            {
                throw new DirectoryNotFoundException($"Author with name: {firstName + ' ' + lastName} not found.");
            }

            return existingAuthor.Id;
        }
    }
}
