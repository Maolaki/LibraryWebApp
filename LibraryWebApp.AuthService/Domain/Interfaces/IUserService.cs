using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Domain.Entities;

namespace LibraryWebApp.AuthService.Domain.Interfaces
{
    public interface IUserService
    {
        public void RegisterNewUser(UserDTO userDto);
        public AuthenticatedResponse? AuthenticateUser(LoginDTO loginModel);
    }
}
