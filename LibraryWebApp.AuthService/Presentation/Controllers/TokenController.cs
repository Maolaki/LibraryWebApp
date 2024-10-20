using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh([FromBody] AuthenticatedResponse authenticatedResponse)
    {
        if (authenticatedResponse is null || authenticatedResponse.AccessToken is null || authenticatedResponse.RefreshToken is null)
            return BadRequest("Invalid client request");

        var result = _tokenService.RefreshTokens(authenticatedResponse);
        if (result is null)
            return BadRequest("Invalid access token or refresh token.");

        return Ok(result);
    }

    [HttpPost, Authorize]
    [Route("revoke")]
    public IActionResult Revoke([FromBody] string refreshToken)
    {
        var username = User.Identity?.Name;

        if (username == null) return BadRequest("User not authenticated.");

        var success = _tokenService.RevokeToken(refreshToken, username);
        if (!success) return BadRequest("Invalid refresh token");

        return Ok();
    }

    [HttpPost, Authorize]
    [Route("revoke-all")]
    public IActionResult RevokeAll()
    {
        var username = User.Identity?.Name;

        if (username == null) return BadRequest("User not authenticated.");

        var success = _tokenService.RevokeAllTokens(username);
        if (!success) return BadRequest("No refresh tokens found for this user");

        return Ok();
    }
}