using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, Book>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public GetBookByIdHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Books.GetAsync(b => b.Id == request.Id);
            if (existingBook == null)
            {
                throw new NotFoundException($"Book with Id {request.Id} not found.");
            }

            return await Task.FromResult(existingBook);
        }
    }
}
