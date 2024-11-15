using MediatR;
using System.Security.Claims;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public record GetUserIdQuery(ClaimsPrincipal ClaimsPrincipalIdentity) : IRequest<long>;
}
