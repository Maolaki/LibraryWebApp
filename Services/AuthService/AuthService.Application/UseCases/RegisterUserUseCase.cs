using AutoMapper;
using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using System.Data;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class RegisterUserUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserUseCase(IMapper mapper, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public void Execute(UserDTO userDto)
        {
            var existingUser = _unitOfWork.Users.Get(u => u.Email == userDto.Email || u.Username == userDto.Username || u.Email == userDto.Username);
            if (existingUser != null)
            {
                throw new DuplicateNameException("User with this email/username already exists.");
            }

            userDto.Password = _passwordHasher.HashPassword(userDto.Password!);

            var newUser = _mapper.Map<User>(userDto);

            _unitOfWork.Users.Create(newUser);
            _unitOfWork.Save();
        }
    }
}
