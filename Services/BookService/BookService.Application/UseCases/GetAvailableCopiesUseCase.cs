using LibraryWebApp.BookService.Application.Interfaces;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetAvailableCopiesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAvailableCopiesUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int Execute(string isbn)
        {
            var books = _unitOfWork.Books.GetAll().Where(b => b.ISBN == isbn).ToList();

            if (!books.Any())
            {
                throw new DirectoryNotFoundException($"Book with ISBN {isbn} not found.");
            }

            int totalCopies = books.Count;
            int borrowedCopies = books.Count(b => b.UserId != null);
            int availableCopies = totalCopies - borrowedCopies;

            return availableCopies;
        }
    }
}
