using LibraryWebApp.AuthService.Domain.Entities;
using System.Security.Claims;

namespace LibraryWebApp.AuthService.Application.Interfaces
{
    public interface ITokenService
    {
        List<Claim> GenerateClaims(User user);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}