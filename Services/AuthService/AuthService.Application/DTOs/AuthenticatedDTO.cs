namespace LibraryWebApp.AuthService.Application.Entities
{
    public class AuthenticatedDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
