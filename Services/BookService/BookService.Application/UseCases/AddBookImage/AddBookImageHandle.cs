using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Exceptions;
using LibraryWebApp.BookService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class AddBookImageHandler : IRequestHandler<AddBookImageCommand, Unit>
    {
        private readonly IUnitOfWork<ImageDTO> _unitOfWork;

        public AddBookImageHandler(IUnitOfWork<ImageDTO> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddBookImageCommand request, CancellationToken cancellationToken)
        {
            var existingBook = await _unitOfWork.Books.GetAsync(b => b.Id == request.BookId);
            if (existingBook == null)
            {
                throw new NotFoundException($"Book with ID {request.BookId} was not found.");
            }

            var imageDto = ConvertToImageDto(request.ImageFile);
            existingBook.Image = imageDto.Image;
            existingBook.ImageContentType = imageDto.ImageContentType;

            _unitOfWork.BookRepositoryWrapper.SetCacheBookImage(request.BookId, imageDto);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }

        private ImageDTO ConvertToImageDto(IFormFile file)
        {
            var imageDto = new ImageDTO
            {
                Image = ConvertToByteArray(file),
                ImageContentType = file.ContentType
            };

            if (imageDto.Image == null || imageDto.ImageContentType == null)
            {
                throw new FormatException("Failed to convert image file to ImageDTO.");
            }

            return imageDto;
        }

        private byte[] ConvertToByteArray(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
