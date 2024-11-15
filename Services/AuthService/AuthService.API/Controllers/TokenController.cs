using MediatR;
using LibraryWebApp.AuthService.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.AuthService.Application.Entities;
using LibraryWebApp.AuthService.API.Filters;

[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IMediator _mediator;

    public TokenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("revoke")]
    [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
    public async Task<IActionResult> RevokeToken([FromBody] string refreshToken)
    {
        await _mediator.Send(new RevokeTokenCommand(refreshToken, User));
        return Ok();
    }

    [HttpPost("revoke-all")]
    [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
    public async Task<IActionResult> RevokeAllTokens()
    {
        await _mediator.Send(new RevokeAllTokensCommand(User));
        return Ok();
    }

    [HttpPost("refresh")]
    [Authorize, ServiceFilter(typeof(ValidateModelAttribute))]
    public async Task<IActionResult> RefreshTokens([FromBody] AuthenticatedDTO authenticatedResponse)
    {
        var newAccessToken = await _mediator.Send(new RefreshTokensCommand(authenticatedResponse));
        return Ok(new { AccessToken = newAccessToken });
    }
}
