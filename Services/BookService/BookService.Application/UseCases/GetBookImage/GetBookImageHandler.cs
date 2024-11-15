using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class GetBookImageHandler : IRequestHandler<GetBookImageQuery, ImageDTO>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public GetBookImageHandler(IUnitOfWork<ImageDTO> unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        public async Task<ImageDTO> Handle(GetBookImageQuery request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Books.GetAsync(b => b.Id == request.BookId);
            if (existingBook == null)
            {
                throw new NotFoundException($"Book with ID {request.BookId} was not found.");
            }

            var imageDto = _unitOfWork.BookRepositoryWrapper.GetCacheBookImage(request.BookId);

            if (imageDto == null)
            {
                ValidateBookImage(existingBook, request.BookId);

                imageDto = new ImageDTO
                {
                    Image = existingBook.Image,
                    ImageContentType = existingBook.ImageContentType
                };

                _unitOfWork.BookRepositoryWrapper.SetCacheBookImage(request.BookId, imageDto);
            }

            return await Task.FromResult(imageDto);
        }

        private void ValidateBookImage(Book? book, int bookId)
        {
            if (book == null || book.Image == null || string.IsNullOrEmpty(book.ImageContentType))
            {
                throw new NotFoundException($"Image for book with ID {bookId} was not found.");
            }
        }
    }
}
