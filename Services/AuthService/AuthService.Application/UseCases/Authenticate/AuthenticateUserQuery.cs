using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Application.Entities;
using MediatR;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public record AuthenticateUserQuery(LoginDTO LoginDto) : IRequest<AuthenticatedDTO>;
}
