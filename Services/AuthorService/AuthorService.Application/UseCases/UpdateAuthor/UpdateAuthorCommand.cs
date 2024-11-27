using LibraryWebApp.AuthorService.Domain.Enums;
using MediatR;

namespace LibraryWebApp.AuthorService.Application.UseCases
{
    public record UpdateAuthorCommand(
    int Id,
    string? FirstName,
    string? LastName,
    DateOnly? DateOfBirth,
    Country? Country)
        : IRequest<Unit>;
}