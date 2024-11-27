using MediatR;
using LibraryWebApp.AuthService.Domain.Enums;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public record RegisterUserCommand(
        string? Username,
        string? Email,
        string? Password,
        UserRole Role = UserRole.User
    ) : IRequest<Unit>;
}
