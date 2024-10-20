using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Enums;
using LibraryWebApp.AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _tokenServiceMock = new Mock<ITokenService>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            _authController = new AuthController(
                _unitOfWorkMock.Object,
                _tokenServiceMock.Object,
                _userServiceMock.Object,
                _passwordHasherMock.Object
            );
        }

        [Fact]
        public void Register_ValidUser_ReturnsOk()
        {
            var userDto = new UserDTO
            {
                Username = "Kapibara",
                Email = "validUser@gmail.com",
                Password = "111111!q",
                Role = UserRole.User
            };

            _userServiceMock.Setup(us => us.RegisterNewUser(userDto)).Verifiable();

            var result = _authController.Register(userDto);

            Assert.IsType<OkResult>(result);
            _userServiceMock.Verify(us => us.RegisterNewUser(userDto), Times.Once);
        }

        [Fact]
        public void Register_InvalidUser_ReturnsBadRequest()
        {
            var userDto = new UserDTO();

            var result = _authController.Register(userDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsAssignableFrom<List<FluentValidation.Results.ValidationFailure>>(badRequestResult.Value);
            Assert.NotEmpty(errors);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsOk()
        {
            var loginDto = new LoginDTO
            {
                Login = "Kapibara",
                Password = "111111!q"
            };

            var authResponse = new AuthenticatedResponse
            {
                AccessToken = "testAccessToken",
                RefreshToken = "testRefreshToken"
            };

            _userServiceMock.Setup(us => us.AuthenticateUser(loginDto)).Returns(authResponse);

            var result = _authController.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(authResponse, okResult.Value);
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var invalidLogin = new LoginDTO
            {
                Login = "wrongUser",
                Password = "111111!11111qqqqq"
            };

            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(s => s.AuthenticateUser(invalidLogin)).Returns((AuthenticatedResponse)null);
            var controller = new AuthController(null, null, mockUserService.Object, null);

            var result = controller.Login(invalidLogin);

            Assert.IsType<UnauthorizedResult>(result);
        }

    }

}