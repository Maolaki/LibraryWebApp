using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryWebApp.BookService.Domain.Enums;

namespace LibraryWebApp.BookService.Domain.Entities
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Country Country { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
