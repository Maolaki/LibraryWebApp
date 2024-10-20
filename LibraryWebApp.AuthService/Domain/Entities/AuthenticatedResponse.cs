namespace LibraryWebApp.AuthService.Domain.Entities
{
    public class AuthenticatedResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
