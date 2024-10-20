using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace UnitTests
{
    public class TokenControllerTests
    {
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly TokenController _tokenController;

        public TokenControllerTests()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _tokenController = new TokenController(_tokenServiceMock.Object);
        }

        [Fact]
        public void Refresh_ValidTokens_ReturnsOk()
        {
            // Arrange
            var authResponse = new AuthenticatedResponse
            {
                AccessToken = "validAccessToken",
                RefreshToken = "validRefreshToken"
            };

            var refreshedTokens = new AuthenticatedResponse
            {
                AccessToken = "newAccessToken",
                RefreshToken = "newRefreshToken"
            };

            _tokenServiceMock.Setup(ts => ts.RefreshTokens(authResponse)).Returns(refreshedTokens);

            // Act
            var result = _tokenController.Refresh(authResponse);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(refreshedTokens, okResult.Value);
        }

        [Fact]
        public void Refresh_InvalidTokens_ReturnsBadRequest()
        {
            // Arrange
            var authResponse = new AuthenticatedResponse
            {
                AccessToken = "invalidAccessToken",
                RefreshToken = "invalidRefreshToken"
            };

            _tokenServiceMock.Setup(ts => ts.RefreshTokens(authResponse)).Returns((AuthenticatedResponse)null);

            // Act
            var result = _tokenController.Refresh(authResponse);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void RevokeAll_ValidUser_ReturnsOk()
        {
            // Arrange
            var username = "testuser";
            _tokenController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, username) })) }
            };

            _tokenServiceMock.Setup(ts => ts.RevokeAllTokens(username)).Returns(true);

            // Act
            var result = _tokenController.RevokeAll();

            // Assert
            Assert.IsType<OkResult>(result);
        }

    }

}
