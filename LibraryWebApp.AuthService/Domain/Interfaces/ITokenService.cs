using System.Security.Claims;
using LibraryWebApp.AuthService.Domain.Entities;

namespace LibraryWebApp.AuthService.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        AuthenticatedResponse? RefreshTokens(AuthenticatedResponse authenticatedResponse);
        bool RevokeToken(string refreshToken, string username);
        bool RevokeAllTokens(string username);
    }
}