using AutoMapper;
using LibraryWebApp.AuthorService.Application.DTOs;
using LibraryWebApp.AuthorService.Application.Validators;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Enums;
using LibraryWebApp.AuthorService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.AuthorService.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorService authorService, IMapper mapper)
        {
            _authorService = authorService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<Author>> GetAllAuthors(int pageNumber = 1, int pageSize = 10)
        {
            var authors = _authorService.GetAllAuthors(pageNumber, pageSize);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Author> GetAuthor(int id)
        {
            var author = _authorService.GetAuthor(id);
            if (author == null) return NotFound();
            return Ok(author);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult AddAuthor([FromBody] AuthorDTO authorDto)
        {
            var validator = new AuthorDTOValidator();
            var result = validator.Validate(authorDto);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var author = _mapper.Map<Author>(authorDto);
            _authorService.AddAuthor(author);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult UpdateAuthor(int id, [FromBody] AuthorDTO authorDto)
        {
            var validator = new AuthorDTOValidator();
            var result = validator.Validate(authorDto);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            var author = _mapper.Map<Author>(authorDto);
            author.Id = id;

            _authorService.UpdateAuthor(author);

            return Ok("Author updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public ActionResult DeleteAuthor(int id)
        {
            var author = _authorService.GetAuthor(id);
            if (author == null) return NotFound();
            _authorService.DeleteAuthor(author);
            return Ok();
        }

        [HttpGet("{id}/books")]
        [Authorize]
        public ActionResult<IEnumerable<Book>> GetAllBooksByAuthor(int id, int pageNumber = 1, int pageSize = 10)
        {
            var author = _authorService.GetAuthor(id);
            if (author == null) return NotFound();

            var books = _authorService.GetAllBooksByAuthor(author, pageNumber, pageSize);
            return Ok(books);
        }
    }
}
