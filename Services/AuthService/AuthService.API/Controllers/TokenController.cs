using AuthService.API.Filters;
using LibraryWebApp.AuthService.Application.UseCases;
using LibraryWebApp.AuthService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly RefreshTokensUseCase _refreshTokensUseCase;
    private readonly RevokeTokenUseCase _revokeTokenUseCase;
    private readonly RevokeAllTokensUseCase _revokeAllTokensUseCase;

    public TokenController(
        RefreshTokensUseCase refreshTokensUseCase,
        RevokeTokenUseCase revokeTokenUseCase,
        RevokeAllTokensUseCase revokeAllTokensUseCase)
    {
        _refreshTokensUseCase = refreshTokensUseCase;
        _revokeTokenUseCase = revokeTokenUseCase;
        _revokeAllTokensUseCase = revokeAllTokensUseCase;
    }

    [HttpPost("refresh")]
    [ServiceFilter(typeof(ValidateModelAttribute))]
    public IActionResult Refresh([FromBody] AuthenticatedDTO authenticatedDTO)
    {
        var result = _refreshTokensUseCase.Execute(authenticatedDTO);
        return Ok(result);
    }

    [HttpPost("revoke")]
    [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
    public IActionResult Revoke([FromBody] string refreshToken)
    {
        _revokeTokenUseCase.Execute(refreshToken, User);
        return Ok();
    }

    [HttpPost("revoke-all")]
    [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
    public IActionResult RevokeAll()
    {
        _revokeAllTokensUseCase.Execute(User);
        return Ok();
    }
}