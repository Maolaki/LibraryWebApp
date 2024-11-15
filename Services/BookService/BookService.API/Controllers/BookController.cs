using AutoMapper;
using LibraryWebApp.BookService.API.Filters;
using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Application.UseCases;
using LibraryWebApp.BookService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LibraryWebApp.BookService.Domain.Entities;

namespace BookService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BookController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks(int pageNumber = 1, int pageSize = 10)
        {
            var query = new GetAllBooksQuery(pageNumber, pageSize);
            var books = await _mediator.Send(query);
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("filtered/")]
        public async Task<ActionResult> GetAllBooksWithFilters(int pageNumber = 1, int pageSize = 10, string? title = null, int? authorId = null, BookGenre? genre = null)
        {
            var query = new GetAllBooksWithFiltersQuery(pageNumber, pageSize, title, genre, authorId);
            var books = await _mediator.Send(query);
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            var query = new GetBookByIdQuery(id);
            var book = await _mediator.Send(query);
            var bookDto = _mapper.Map<BookDTO>(book);
            return Ok(bookDto);
        }

        [HttpGet("isbn/{isbn}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByISBN(string isbn)
        {
            var query = new GetBooksByISBNQuery(isbn);
            var books = await _mediator.Send(query);
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }

        [HttpGet("{isbn}/availableCopies")]
        public async Task<ActionResult<int>> GetAvailableCopies(string isbn)
        {
            var query = new GetAvailableCopiesQuery(isbn);
            var availableCopies = await _mediator.Send(query);
            return Ok(availableCopies);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin)), ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<ActionResult> AddBook([FromBody] BookDTO bookDto)
        {
            var command = new AddBookCommand(_mapper.Map<Book>(bookDto));
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin)), ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<ActionResult> UpdateBook(int id, [FromBody] BookDTO bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            book.Id = id;
            var command = new UpdateBookCommand(book);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var command = new DeleteBookCommand(id);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("return/{id}")]
        [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
        public async Task<ActionResult> ReturnBook(int id)
        {
            var command = new ReturnBookCommand(User, id);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("checkout/{id}")]
        [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
        public async Task<ActionResult> CheckoutBook(int id)
        {
            var command = new CheckoutBookCommand(id, User);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost("image/{bookId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult> AddBookImage(int bookId, IFormFile imageFile)
        {
            var command = new AddBookImageCommand(bookId, imageFile);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("image/{bookId}")]
        public async Task<ActionResult> GetBookImage(int bookId)
        {
            var query = new GetBookImageQuery(bookId);
            var imageDto = await _mediator.Send(query);
            return File(imageDto.Image!, imageDto.ImageContentType!);
        }

        [HttpPost("notify/{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult> NotifyCheckoutDate(int id)
        {
            var command = new NotifyCheckoutDateCommand(id);
            await _mediator.Send(command);
            return Ok();
        }
    }
}
