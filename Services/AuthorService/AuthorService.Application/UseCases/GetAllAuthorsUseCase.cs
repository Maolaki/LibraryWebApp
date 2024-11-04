using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public class GetAllAuthorsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllAuthorsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Author> Execute(int pageNumber, int pageSize)
        {
            return _unitOfWork.Authors.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Include(a => a.Books)
                .Take(pageSize);
        }
    }
}
