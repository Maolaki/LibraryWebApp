using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryWebApp.BookService.Domain.Enums;
using System.Text.Json.Serialization;

namespace LibraryWebApp.BookService.Domain.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ISBN {  get; set; } 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public BookGenre Genre { get; set; }
        public int AuthorId { get; set; }
        public long? UserId { get; set; }
        public DateTime? CheckoutDateTime { get; set; }
        public DateTime? ReturnDateTime { get; set; }
        public byte[]? Image { get; set; }
        public string? ImageContentType { get; set; }

        [JsonIgnore]
        public virtual Author? Author { get; set; }
        public virtual User? User { get; set; }
    }
}
