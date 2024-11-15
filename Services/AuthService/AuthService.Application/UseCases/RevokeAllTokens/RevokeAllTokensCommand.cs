using MediatR;
using System.Security.Claims;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public record RevokeAllTokensCommand(ClaimsPrincipal User) : IRequest<Unit>;
}
