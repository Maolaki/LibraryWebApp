using LibraryWebApp.AuthService.Domain.Entities;
using LibraryWebApp.AuthService.Domain.Interfaces;
using System.Security.Claims;

namespace LibraryWebApp.AuthService.Application.UseCases
{
    public class GetUserIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User Execute(ClaimsPrincipal claimsPrincipalIdentity)
        {
            var username = claimsPrincipalIdentity.Identity?.Name;

            if (username is null)
                throw new DirectoryNotFoundException("Wrong username");

            var existingUser = _unitOfWork.Users.Get(u => u.Username == username);

            if (existingUser is null)
                throw new DirectoryNotFoundException("Wrong username");

            return existingUser;
        }
    }
}
