using LibraryWebApp.AuthService.Domain.Enums;

namespace LibraryWebApp.AuthService.Application.DTOs
{
    public class UserDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}
