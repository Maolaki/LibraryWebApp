using LibraryWebApp.AuthorService.Application.Filters;
using AutoMapper;
using LibraryWebApp.AuthorService.Application.DTOs;
using LibraryWebApp.AuthorService.Application.UseCases;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.AuthorService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly GetAuthorIdUseCase _getAuthorIdUseCase;
        private readonly GetAllAuthorsUseCase _getAllAuthorsUseCase;
        private readonly GetAuthorUseCase _getAuthorUseCase;
        private readonly AddAuthorUseCase _addAuthorUseCase;
        private readonly UpdateAuthorUseCase _updateAuthorUseCase;
        private readonly DeleteAuthorUseCase _deleteAuthorUseCase;
        private readonly GetAllBooksByAuthorUseCase _getAllBooksByAuthorUseCase;
        private readonly IMapper _mapper;

        public AuthorController(
            GetAuthorIdUseCase getAuthorIdUseCase,
            GetAllAuthorsUseCase getAllAuthorsUseCase,
            GetAuthorUseCase getAuthorUseCase,
            AddAuthorUseCase addAuthorUseCase,
            UpdateAuthorUseCase updateAuthorUseCase,
            DeleteAuthorUseCase deleteAuthorUseCase,
            GetAllBooksByAuthorUseCase getAllBooksByAuthorUseCase,
            IMapper mapper)
        {
            _getAuthorIdUseCase = getAuthorIdUseCase;
            _getAllAuthorsUseCase = getAllAuthorsUseCase;
            _getAuthorUseCase = getAuthorUseCase;
            _addAuthorUseCase = addAuthorUseCase;
            _updateAuthorUseCase = updateAuthorUseCase;
            _deleteAuthorUseCase = deleteAuthorUseCase;
            _getAllBooksByAuthorUseCase = getAllBooksByAuthorUseCase;
            _mapper = mapper;
        }

        [HttpGet("by-name")]
        public ActionResult<int> GetAuthorId(string firstName, string lastName)
        {
            var authorId = _getAuthorIdUseCase.Execute(firstName, lastName);
            return Ok(authorId);
        }

        [HttpGet]
        public ActionResult<IEnumerable<AuthorDTO>> GetAllAuthors(int pageNumber = 1, int pageSize = 10)
        {
            var authors = _getAllAuthorsUseCase.Execute(pageNumber, pageSize);
            var authorsDto = _mapper.Map<IEnumerable<AuthorDTO>>(authors);
            return Ok(authorsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<AuthorDTO> GetAuthor(int id)
        {
            var author = _getAuthorUseCase.Execute(id);
            var authorDto = _mapper.Map<AuthorDTO>(author);
            return Ok(authorDto);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin)), ServiceFilter(typeof(ValidateModelAttribute))]
        public ActionResult AddAuthor([FromBody] AuthorDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            _addAuthorUseCase.Execute(author);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin)), ServiceFilter(typeof(ValidateModelAttribute))]
        public ActionResult UpdateAuthor(int authorId, [FromBody] AuthorDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            author.Id = authorId;

            _updateAuthorUseCase.Execute(author);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult DeleteAuthor(int authorId)
        {
            _deleteAuthorUseCase.Execute(authorId);
            return Ok();
        }

        [HttpGet("{id}/books")]
        public ActionResult<IEnumerable<BookDTO>> GetAllBooksByAuthor(int authorId, int pageNumber = 1, int pageSize = 10)
        {
            var books = _getAllBooksByAuthorUseCase.Execute(authorId, pageNumber, pageSize);
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }
    }
}
