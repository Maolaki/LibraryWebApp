using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryWebApp.AuthService.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secret = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException("JWT secret is not configured.");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var secret = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException("JWT secret is not configured.");
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (!(securityToken is JwtSecurityToken jwtToken) || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var filteredClaims = principal.Claims
                .Where(c => c.Type == ClaimTypes.Name || c.Type == ClaimTypes.Role);

            var identity = new ClaimsIdentity(filteredClaims, "jwt");

            return new ClaimsPrincipal(identity);
        }

        public AuthenticatedResponse? RefreshTokens(AuthenticatedResponse authenticatedResponse)
        {
            var handler = new JwtSecurityTokenHandler();
            var principal = GetPrincipalFromExpiredToken(authenticatedResponse.AccessToken!);
            var username = principal?.Identity?.Name;

            if (username == null || principal == null)
                return null;

            var refToken = _unitOfWork.RefreshTokens.Get(t => t.Token == authenticatedResponse.RefreshToken && t.User!.Username == username);
            if (refToken == null || refToken.RefreshTokenExpiryTime <= DateTime.Now)
                return null;

            var newAccessToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();

            refToken.Token = newRefreshToken;
            _unitOfWork.Save();

            return new AuthenticatedResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public bool RevokeToken(string refreshToken, string username)
        {
            var refToken = _unitOfWork.RefreshTokens.Get(t => t.Token == refreshToken && t.User!.Username == username);
            if (refToken == null)
                return false;

            _unitOfWork.RefreshTokens.Delete(refToken);
            _unitOfWork.Save();
            return true;
        }

        public bool RevokeAllTokens(string username)
        {
            var refreshTokens = _unitOfWork.RefreshTokens.GetAll()
                .Include(t => t.User)
                .Where(t => t.User != null && t.User.Username == username)
                .ToList();

            if (!refreshTokens.Any())
                return false;

            foreach (var refreshToken in refreshTokens)
                _unitOfWork.RefreshTokens.Delete(refreshToken);

            _unitOfWork.Save();
            return true;
        }
    }
}