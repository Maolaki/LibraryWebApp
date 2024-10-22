using LibraryWebApp.BookService.Domain.Enums;

namespace LibraryWebApp.BookService.Application.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public BookGenre Genre { get; set; }
        public int AuthorId { get; set; }
        public long? UserId { get; set; }
        public DateTime? CheckoutDateTime { get; set; }
        public DateTime? ReturnDateTime { get; set; }
    }
}
