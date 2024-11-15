using LibraryWebApp.AuthService.Domain.Interfaces;
using System.Security.Claims;
using LibraryWebApp.AuthService.Application.Entities;
using LibraryWebApp.AuthService.Domain.Entities;
using MediatR;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, AuthenticatedDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticateUserHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticatedDTO> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            var loginDto = request.LoginDto;
            var user = await _unitOfWork.Users.GetAsync(u => u.Username == loginDto.Login || u.Email == loginDto.Login);

            if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password!, user.HashedPassword!))
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
                RefreshTokenExpiryTime = DateTime.Now.AddDays(1)
            });

            await _unitOfWork.SaveAsync();

            return new AuthenticatedDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
