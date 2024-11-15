using MediatR;
using System.Security.Claims;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public record RevokeTokenCommand(string RefreshToken, ClaimsPrincipal User) : IRequest<Unit>;
}
