using MediatR;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public record RefreshTokensCommand(
        string? AccessToken,
        string? RefreshToken
    ) : IRequest<string>;
}
