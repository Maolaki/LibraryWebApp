﻿using Microsoft.Extensions.Caching.Memory;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Interfaces;
using LibraryWebApp.BookService.Application.DTOs;

namespace LibraryWebApp.BookService.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailService _emailService;

        public BookService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _emailService = emailService;
        }

        public IEnumerable<Book> GetAllBooks(int pageNumber, int pageSize)
        {
            return _unitOfWork.Books.GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public Book GetBook(int id)
        {
            return _unitOfWork.Books.Get(b => b.Id == id);
        }

        public IEnumerable<Book> GetBookByISBN(string isbn)
        {
            return _unitOfWork.Books.GetAll()
                .Where(b => b.ISBN == isbn)
                .ToList(); 
        }

        public void AddBook(Book book)
        {
            _unitOfWork.Books.Create(book);
            _unitOfWork.Save();
        }

        public void UpdateBook(Book book)
        {
            _unitOfWork.Books.Update(book);
            _unitOfWork.Save();
        }

        public void DeleteBook(Book book)
        {
            _unitOfWork.Books.Delete(book);
            _unitOfWork.Save();
        }

        public void ReturnBook(string username,int id)
        {
            var user = _unitOfWork.Users.Get(u => u.Username == username);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var book = _unitOfWork.Books.Get(b => b.Id == id);

            if (book == null)
            {
                throw new Exception("Book not found.");
            }

            if (book.UserId == null)
            {
                throw new Exception("Book is not currently checked out.");
            }

            if (book.User != user)
            {
                throw new Exception("User have no access.");
            }

            book.UserId = null;
            book.CheckoutDateTime = null;
            book.ReturnDateTime = null;

            _unitOfWork.Save();
        }

        public void CheckoutBook(int bookId, string username)
        {
            var user = _unitOfWork.Users.Get(u => u.Username == username);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var book = _unitOfWork.Books.Get(b => b.Id == bookId);

            if (book != null)
            {
                DateTime dateTime = DateTime.Now;
                book.CheckoutDateTime = dateTime;
                book.ReturnDateTime = dateTime.AddDays(14);
                book.UserId = user.Id;
                _unitOfWork.Save(); 
            }
        }

        public void AddBookImage(int bookId, ImageDTO imageDto)
        {
            var book = _unitOfWork.Books.Get(b => b.Id == bookId);
            if (book != null)
            {
                book.Image = imageDto.Image;
                book.ImageContentType = imageDto.ImageContentType;

                var cacheKey = $"book-image-{bookId}";
                _memoryCache.Set(cacheKey, imageDto.Image, TimeSpan.FromDays(1));

                _unitOfWork.Save(); 
            }
        }

        public ImageDTO? GetBookImage(int bookId)
        {
            var cacheKey = $"book-image-{bookId}";
            if (_memoryCache.TryGetValue(cacheKey, out ImageDTO? cachedImage))
            {
                return cachedImage;
            }

            var book = _unitOfWork.Books.Get(b => b.Id == bookId);
            if (book?.Image != null)
            {
                var imageDto = new ImageDTO
                {
                    Image = book.Image,
                    ImageContentType = book.ImageContentType
                };

                _memoryCache.Set(cacheKey, imageDto, TimeSpan.FromDays(2));
                return imageDto;
            }

            return null;
        }

        public void NotifyCheckoutDate(int id)
        {
            var book = _unitOfWork.Books.Get(b => b.Id == id);
            if (book != null && book.ReturnDateTime <= DateTime.Now)
            {
                var user = _unitOfWork.Users.Get(u => u.Id == book.UserId);
                if (user != null)
                {
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        throw new ArgumentNullException(nameof(user.Email), "User email must be provided.");
                    }

                    var subject = "Напоминание по возвращению книги!";
                    var body = $"Дорогай {user.Username},<br/><br/>" +
                               $"Это напоминание, что книгу '{book.Title}' Вы должны были вернуть {book.ReturnDateTime.ToString()}.<br/><br/>" +
                               "Пожалуйста, верните книгу, если не хотите проблем.<br/><br/>" +
                               "Спасибо вам :>";
                    _emailService.SendEmail(user.Email, subject, body);
                }
            }
        }

        public byte[] ConvertToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}