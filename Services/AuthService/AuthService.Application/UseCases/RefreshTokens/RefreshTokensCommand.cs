using MediatR;
using LibraryWebApp.AuthService.Application.Entities;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public record RefreshTokensCommand(AuthenticatedDTO AuthenticatedResponse) : IRequest<string>;
}
