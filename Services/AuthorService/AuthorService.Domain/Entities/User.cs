using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryWebApp.AuthorService.Domain.Enums;

namespace LibraryWebApp.AuthorService.Domain.Entities
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

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
