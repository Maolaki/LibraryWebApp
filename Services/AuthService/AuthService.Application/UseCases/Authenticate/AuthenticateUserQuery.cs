using MediatR;
using LibraryWebApp.AuthService.Application.Entities;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public record AuthenticateUserQuery(
        string? Login,
        string? Password
    ) : IRequest<AuthenticatedDTO>;
}
