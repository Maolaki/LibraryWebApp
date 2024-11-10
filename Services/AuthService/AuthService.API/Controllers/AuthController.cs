using LibraryWebApp.AuthService.Application.Filters;
using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly AuthenticateUserUseCase _authenticateUserUseCase;
    private readonly GetUserIdUseCase _getUserIdUseCase;

    public AuthController(
        RegisterUserUseCase registerUserUseCase,
        AuthenticateUserUseCase authenticateUserUseCase,
        GetUserIdUseCase getUserIdUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _getUserIdUseCase = getUserIdUseCase;
        _authenticateUserUseCase = authenticateUserUseCase;
    }

    [HttpPost("register")]
    [ServiceFilter(typeof(ValidateModelAttribute))]
    public IActionResult Register([FromBody] UserDTO userDto)
    {
        _registerUserUseCase.Execute(userDto);
        return Ok();
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidateModelAttribute))]
    public IActionResult Login([FromBody] LoginDTO loginDto)
    {
        var response = _authenticateUserUseCase.Execute(loginDto);
        return Ok(response);
    }

    [HttpGet("get-id")]
    [Authorize, ServiceFilter(typeof(EnsureAuthenticatedUserFilter))]
    public ActionResult<int> GetId()
    {
        var user = _getUserIdUseCase.Execute(User);
        return Ok(user.Id);
    }
}
