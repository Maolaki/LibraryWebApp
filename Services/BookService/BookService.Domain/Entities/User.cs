using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryWebApp.BookService.Domain.Enums;

namespace LibraryWebApp.BookService.Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? HashedPassword { get; set; }
        public UserRole Role { get; set; } = UserRole.User;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
