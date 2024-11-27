using LibraryWebApp.AuthorService.Domain.Enums;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public record AddAuthorCommand(
    int Id,
    string? FirstName,
    string? LastName,
    DateOnly DateOfBirth,
    Country Country) 
        : IRequest<Unit>;
}
