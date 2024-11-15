using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using LibraryWebApp.AuthService.API.Filters;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ServiceFilter(typeof(ValidateModelAttribute))]
    public async Task<IActionResult> Register([FromBody] UserDTO userDto)
    {
        await _mediator.Send(new RegisterUserCommand(userDto));
        return Ok();
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidateModelAttribute))]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var response = await _mediator.Send(new AuthenticateUserQuery(loginDto));
        return Ok(response);
    }

    [HttpGet("get-id")]
    [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
    public async Task<ActionResult<int>> GetId()
    {
        var userId = await _mediator.Send(new GetUserIdQuery(User));
        return Ok(userId);
    }
}
