using LibraryWebApp.BookService.Application.Interfaces;
using LibraryWebApp.BookService.Domain.Entities;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetBookByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBookByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Book Execute(int id)
        {
            var existingBook = _unitOfWork.Books.Get(b => b.Id == id);
            if (existingBook == null)
            {
                throw new DirectoryNotFoundException($"Book with Id {id} not found.");
            }

            return existingBook;
        }
    }
}
