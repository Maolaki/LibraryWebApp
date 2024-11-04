using LibraryWebApp.AuthorService.Domain.Enums;

namespace LibraryWebApp.AuthorService.Application.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Country Country { get; set; }
    }
}
