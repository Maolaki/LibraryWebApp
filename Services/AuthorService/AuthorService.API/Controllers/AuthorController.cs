using AutoMapper;
using LibraryWebApp.AuthorService.Application.DTOs;
using LibraryWebApp.AuthorService.Application.UseCases;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.AuthorService.API.Filters;

namespace LibraryWebApp.AuthorService.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthorController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("by-name")]
        public async Task<ActionResult<int>> GetAuthorId(string firstName, string lastName)
        {
            var query = new GetAuthorIdQuery(firstName, lastName);
            var authorId = await _mediator.Send(query);
            return Ok(authorId);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAllAuthors(int pageNumber = 1, int pageSize = 10)
        {
            var query = new GetAllAuthorsQuery(pageNumber, pageSize);
            var authors = await _mediator.Send(query);
            var authorsDto = _mapper.Map<IEnumerable<AuthorDTO>>(authors);
            return Ok(authorsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
            var query = new GetAuthorQuery(id);
            var author = await _mediator.Send(query);
            var authorDto = _mapper.Map<AuthorDTO>(author);
            return Ok(authorDto);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Admin)), ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<ActionResult> AddAuthor([FromBody] AuthorDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            var command = new AddAuthorCommand(author);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin)), ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] AuthorDTO authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            author.Id = id;
            var command = new UpdateAuthorCommand(author);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var command = new DeleteAuthorCommand(id);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooksByAuthor(int id, int pageNumber = 1, int pageSize = 10)
        {
            var query = new GetAllBooksByAuthorQuery(id, pageNumber, pageSize);
            var books = await _mediator.Send(query);
            var booksDto = _mapper.Map<IEnumerable<BookDTO>>(books);
            return Ok(booksDto);
        }
    }
}
