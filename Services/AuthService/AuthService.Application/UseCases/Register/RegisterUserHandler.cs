using AutoMapper;
using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using MediatR;
using System.Data;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.Users.GetAsync(u => u.Email == request.Email || u.Username == request.Username);
            if (existingUser != null)
            {
                throw new DuplicateNameException("Пользователь с таким email или username уже существует.");
            }

            var newUser = new User
            {
                Username = request.Username,
                HashedPassword = _passwordHasher.HashPassword(request.Password!),
                Email = request.Email,
                Role = request.Role,
            };

            _unitOfWork.Users.Create(newUser);
            await _unitOfWork.SaveAsync(); 

            return Unit.Value;
        }
    }
}
