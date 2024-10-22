using Azure;
using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Application.Validators;
using LibraryWebApp.AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthController(IUnitOfWork unitOfWork, ITokenService tokenService, IUserService userService, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _userService = userService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost, Route("register")]
    public IActionResult Register([FromBody] UserDTO userDto)
    {
        var validator = new UserDTOValidator();
        var result = validator.Validate(userDto);

        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        _userService.RegisterNewUser(userDto);

        return Ok();
    }

    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] LoginDTO loginDto)
    {
        var validator = new LoginDTOValidator();
        var result = validator.Validate(loginDto);

        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        var response = _userService.AuthenticateUser(loginDto);
        if (response is null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }

    [HttpGet("get-id")]
    [Authorize]
    public ActionResult<int> GetId()
    {
        var username = User.Identity?.Name;

        var user = _unitOfWork.Users.Get(u => u.Username == username);

        if (user is null)
        {
            return BadRequest("User is not found.");
        }

        long id = user.Id;

        return Ok(id);
    }
}