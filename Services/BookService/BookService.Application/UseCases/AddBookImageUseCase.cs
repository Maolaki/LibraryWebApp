using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LibraryWebApp.BookService.Application.UseCases
{
    public class AddBookImageUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBookImageUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Execute(int bookId, IFormFile imageFile)
        {
            var existingBook = _unitOfWork.Books.Get(b => b.Id == bookId);
            if (existingBook == null)
            {
                throw new DirectoryNotFoundException($"Book with ID {bookId} was not found.");
            }

            var imageDto = ConvertToImageDto(imageFile);
            existingBook.Image = imageDto.Image;
            existingBook.ImageContentType = imageDto.ImageContentType;
            _unitOfWork.BookRepositoryWrapper.SetCacheBookImage(bookId, imageDto);

            _unitOfWork.Save();
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
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
