using AutoMapper;
using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using MediatR;
using System.Data;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserHandler(IMapper mapper, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = request.UserDto;

            var existingUser = await _unitOfWork.Users.GetAsync(u => u.Email == userDto.Email || u.Username == userDto.Username);
            if (existingUser != null)
            {
                throw new DuplicateNameException("Пользователь с таким email или username уже существует.");
            }

            userDto.Password = _passwordHasher.HashPassword(userDto.Password!);

            var newUser = _mapper.Map<User>(userDto);

            _unitOfWork.Users.Create(newUser);
            await _unitOfWork.SaveAsync(); 

            return Unit.Value;
        }
    }
}
