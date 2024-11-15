using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryWebApp.AuthorService.Domain.Enums;
using System.Text.Json.Serialization;

namespace LibraryWebApp.AuthorService.Domain.Entities
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

        [JsonIgnore]
        public virtual Author? Author { get; set; }
        public virtual User? User { get; set; }
    }
}
