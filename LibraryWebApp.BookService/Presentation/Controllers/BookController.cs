using AutoMapper;
using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.Validators;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Enums;
using LibraryWebApp.BookService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.BookService.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BookController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDTO>> GetAllBooks(int pageNumber = 1, int pageSize = 10)
        {
            var books = _bookService.GetAllBooks(pageNumber, pageSize);

            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);

            return Ok(booksDto);
        }

        [HttpGet("filtered/")]
        public ActionResult GetBooksWithFilters(int pageNumber = 1, int pageSize = 10, string? title = null, int? authorId = null, BookGenre? genre = null)
        {
            var books = _bookService.GetAllBooksWithFilters(pageNumber, pageSize, title, genre, authorId);

            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);

            return Ok(booksDto);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBook(int id)
        {
            var book = _bookService.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BookDTO>(book);

            return Ok(bookDto);
        }

        [HttpGet("isbn/{isbn}")]
        public ActionResult<IEnumerable<BookDTO>> GetBooksByISBN(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                return BadRequest("ISBN cannot be null or empty.");
            }

            var books = _bookService.GetBooksByISBN(isbn);

            if (books == null || !books.Any())
            {
                return NotFound("No books found with the provided ISBN.");
            }

            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);

            return Ok(booksDto);
        }

        [HttpGet("{isbn}/availableCopies")]
        public ActionResult<int> GetAvailableCopies(string isbn)
        {
            var availableCopies = _bookService.GetAvailableCopies(isbn); 
            return Ok(availableCopies); 
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult AddBook([FromBody] BookDTO bookDto)
        {
            var validator = new BookDTOValidator();
            var result = validator.Validate(bookDto);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var book = _mapper.Map<Book>(bookDto);
            _bookService.AddBook(book);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult UpdateBook(int id, [FromBody] BookDTO bookDto)
        {
            var validator = new BookDTOValidator();
            var result = validator.Validate(bookDto);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var book = _mapper.Map<Book>(bookDto);
            book.Id = id;

            _bookService.UpdateBook(book);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult DeleteBook(int id)
        {
            var book = _bookService.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }
            _bookService.DeleteBook(book);
            return Ok();
        }

        [HttpPost("return/{id}")]
        [Authorize]
        public ActionResult ReturnBook(int id)
        {
            if (User.Identity == null || string.IsNullOrEmpty(User.Identity.Name))
            {
                return Unauthorized("User is not authenticated correctly.");
            }

            var username = User.Identity.Name;

            _bookService.ReturnBook(username, id);
            return Ok();
        }

        [HttpPost("checkout/{id}")]
        [Authorize]
        public ActionResult CheckoutBook(int id)
        {
            if (User.Identity == null || string.IsNullOrEmpty(User.Identity.Name))
            {
                return Unauthorized("User is not authenticated correctly.");
            }

            var username = User.Identity.Name;

            _bookService.CheckoutBook(id, username);
            return Ok();
        }

        [HttpPost("image/{bookId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult AddBookImage(int bookId, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var imageDto = new ImageDTO
            {
                Image = _bookService.ConvertToByteArray(imageFile),
                ImageContentType = imageFile.ContentType
            };

            if (imageDto.Image == null || imageDto.ImageContentType == null)
            {
                return BadRequest("Failed to convert image file to ImageDTO.");
            }

            _bookService.AddBookImage(bookId, imageDto);
            return Ok();
        }

        [HttpGet("image/{bookId}")]
        public ActionResult GetBookImage(int bookId)
        {
            var imageDto = _bookService.GetBookImage(bookId);
            if (imageDto == null || imageDto.Image == null || imageDto.Image.Length == 0 || imageDto.ImageContentType == null)
            {
                return NotFound();
            }

            return File(imageDto.Image, imageDto.ImageContentType);
        }

        [HttpPost("notify/{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult NotifyCheckoutDate(int id)
        {
            _bookService.NotifyCheckoutDate(id);
            return Ok();
        }
    }
}