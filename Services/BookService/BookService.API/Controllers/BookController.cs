using AutoMapper;
using LibraryWebApp.BookService.Application.Filters;
using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.UseCases;
using LibraryWebApp.BookService.Domain.Entities;
using LibraryWebApp.BookService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly GetAllBooksUseCase _getAllBooksUseCase;
        private readonly GetAllBooksWithFiltersUseCase _getAllBooksWithFiltersUseCase;
        private readonly GetBookByIdUseCase _getBookByIdUseCase;
        private readonly GetBooksByISBNUseCase _getBooksByISBNUseCase;
        private readonly GetAvailableCopiesUseCase _getAvailableCopiesUseCase;
        private readonly AddBookUseCase _addBookUseCase;
        private readonly UpdateBookUseCase _updateBookUseCase;
        private readonly DeleteBookUseCase _deleteBookUseCase;
        private readonly ReturnBookUseCase _returnBookUseCase;
        private readonly CheckoutBookUseCase _checkoutBookUseCase;
        private readonly AddBookImageUseCase _addBookImageUseCase;
        private readonly GetBookImageUseCase _getBookImageUseCase;
        private readonly NotifyCheckoutDateUseCase _notifyCheckoutDateUseCase;
        private readonly IMapper _mapper;

        public BookController(
            GetAllBooksUseCase getAllBooksUseCase,
            GetAllBooksWithFiltersUseCase getAllBooksWithFiltersUseCase,
            GetBookByIdUseCase getBookByIdUseCase,
            GetBooksByISBNUseCase getBooksByISBNUseCase,
            GetAvailableCopiesUseCase getAvailableCopiesUseCase,
            AddBookUseCase addBookUseCase,
            UpdateBookUseCase updateBookUseCase,
            DeleteBookUseCase deleteBookUseCase,
            ReturnBookUseCase returnBookUseCase,
            CheckoutBookUseCase checkoutBookUseCase,
            AddBookImageUseCase addBookImageUseCase,
            GetBookImageUseCase getBookImageUseCase,
            NotifyCheckoutDateUseCase notifyCheckoutDateUseCase,
            IMapper mapper)
        {
            _getAllBooksUseCase = getAllBooksUseCase;
            _getAllBooksWithFiltersUseCase = getAllBooksWithFiltersUseCase;
            _getBookByIdUseCase = getBookByIdUseCase;
            _getBooksByISBNUseCase = getBooksByISBNUseCase;
            _getAvailableCopiesUseCase = getAvailableCopiesUseCase;
            _addBookUseCase = addBookUseCase;
            _updateBookUseCase = updateBookUseCase;
            _deleteBookUseCase = deleteBookUseCase;
            _returnBookUseCase = returnBookUseCase;
            _checkoutBookUseCase = checkoutBookUseCase;
            _addBookImageUseCase = addBookImageUseCase;
            _getBookImageUseCase = getBookImageUseCase;
            _notifyCheckoutDateUseCase = notifyCheckoutDateUseCase;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDTO>> GetAllBooks(int pageNumber = 1, int pageSize = 10)
        {
            var books = _getAllBooksUseCase.Execute(pageNumber, pageSize);
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("filtered/")]
        public ActionResult GetBooksWithFilters(int pageNumber = 1, int pageSize = 10, string? title = null, int? authorId = null, BookGenre? genre = null)
        {
            var books = _getAllBooksWithFiltersUseCase.Execute(pageNumber, pageSize, title, genre, authorId);
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBook(int id)
        {
            var book = _getBookByIdUseCase.Execute(id);
            var bookDto = _mapper.Map<BookDTO>(book);
            return Ok(bookDto);
        }

        [HttpGet("isbn/{isbn}")]
        public ActionResult<IEnumerable<BookDTO>> GetBooksByISBN(string isbn)
        {
            var books = _getBooksByISBNUseCase.Execute(isbn);
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("{isbn}/availableCopies")]
        public ActionResult<int> GetAvailableCopies(string isbn)
        {
            var availableCopies = _getAvailableCopiesUseCase.Execute(isbn);
            return Ok(availableCopies);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin)), ServiceFilter(typeof(ValidateModelAttribute))]
        public ActionResult AddBook([FromBody] BookDTO bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _addBookUseCase.Execute(book);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin)), ServiceFilter(typeof(ValidateModelAttribute))]
        public ActionResult UpdateBook(int id, [FromBody] BookDTO bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            book.Id = id;

            _updateBookUseCase.Execute(book);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult DeleteBook(int id)
        {
            var book = _getBookByIdUseCase.Execute(id);
            _deleteBookUseCase.Execute(book);
            return Ok();
        }

        [HttpPost("return/{id}")]
        [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
        public ActionResult ReturnBook(int id)
        {
            _returnBookUseCase.Execute(User, id);
            return Ok();
        }

        [HttpPost("checkout/{id}")]
        [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
        public ActionResult CheckoutBook(int id)
        {
            _checkoutBookUseCase.Execute(id, User);
            return Ok();
        }

        [HttpPost("image/{bookId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult AddBookImage(int bookId, IFormFile imageFile)
        {
            _addBookImageUseCase.Execute(bookId, imageFile);
            return Ok();
        }

        [HttpGet("image/{bookId}")]
        public ActionResult GetBookImage(int bookId)
        {
            var imageDto = _getBookImageUseCase.Execute(bookId);
            return File(imageDto.Image!, imageDto.ImageContentType!);
        }

        [HttpPost("notify/{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult NotifyCheckoutDate(int id)
        {
            _notifyCheckoutDateUseCase.Execute(id);
            return Ok();
        }
    }
}
