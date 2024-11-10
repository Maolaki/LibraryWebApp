using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Application.Entities;
using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using System.Security.Claims;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class AuthenticateUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticateUserUseCase(IUnitOfWork unitOfWork, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public AuthenticatedDTO? Execute(LoginDTO loginModel)
        {
            var user = _unitOfWork.Users.Get(u =>
                u.Username == loginModel.Login || u.Email == loginModel.Login);

            if (user is null || !_passwordHasher.VerifyPassword(loginModel.Password!, user.HashedPassword!))
            {
                throw new ArgumentException("Wrong login or/and password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _unitOfWork.RefreshTokens.Create(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(1),
            });

            _unitOfWork.Save();

            return new AuthenticatedDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
