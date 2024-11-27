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
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserQuery loginDto)
    {
        var response = await _mediator.Send(loginDto);
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
