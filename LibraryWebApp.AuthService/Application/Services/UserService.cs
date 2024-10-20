using AutoMapper;
using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using System.Security.Claims;

namespace LibraryWebApp.AuthService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, ITokenService tokenService, IPasswordHasher passwordHasher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public void RegisterNewUser(UserDTO userDto)
        {
            var existingUser = _unitOfWork.Users.Get(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            userDto.Password = _passwordHasher.HashPassword(userDto.Password!);

            var newUser = _mapper.Map<User>(userDto);

            _unitOfWork.Users.Create(newUser);
            _unitOfWork.Save();
        }

        public AuthenticatedResponse? AuthenticateUser(LoginDTO loginModel)
        {
            var user = _unitOfWork.Users.Get(u =>
                u.Username == loginModel.Login || u.Email == loginModel.Login);

            if (user is null || !_passwordHasher.VerifyPassword(loginModel.Password!, user.HashedPassword!))
            {
                return null; 
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

            return new AuthenticatedResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
