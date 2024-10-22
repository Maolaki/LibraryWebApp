using LibraryWebApp.AuthorService.Domain.Entities;

namespace LibraryWebApp.AuthorService.Domain.Interfaces
{
    public interface IAuthorService
    {
        public int GetAuthorId(string firstName, string lastName);
        public IEnumerable<Author> GetAllAuthors(int pageNumber, int pageSize);

        public Author GetAuthor(int id);

        public void AddAuthor(Author author);

        public void UpdateAuthor(Author author);

        public void DeleteAuthor(Author author);

        public IEnumerable<Book> GetAllBooksByAuthor(Author author, int pageNumber, int pageSize);
    }
}
