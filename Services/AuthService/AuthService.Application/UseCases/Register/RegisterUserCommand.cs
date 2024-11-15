using LibraryWebApp.AuthService.Application.DTOs;
using MediatR;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public record RegisterUserCommand(UserDTO UserDto) : IRequest<Unit>;
}
