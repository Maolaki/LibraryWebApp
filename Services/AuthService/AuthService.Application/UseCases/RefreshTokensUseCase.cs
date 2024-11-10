using LibraryWebApp.AuthService.Application.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class RefreshTokensUseCase
    {
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokensUseCase(ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public string? Execute(AuthenticatedDTO authenticatedResponse)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(authenticatedResponse.AccessToken!);
            var username = principal?.Identity?.Name;

            if (username == null || principal == null)
                throw new ArgumentException("Wrong jwt or/and username");

            var refToken = _unitOfWork.RefreshTokens.Get(t => t.Token == authenticatedResponse.RefreshToken && t.User!.Username == username);
            if (refToken == null || refToken.RefreshTokenExpiryTime <= DateTime.Now)
                throw new ArgumentException("Wrong refreshToken");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);

            refToken.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            _unitOfWork.Save();

            return newAccessToken;
        }
    }
}
