using AutoMapper;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Infrastructure.Context;

namespace LibraryWebApp.AuthorService.Infrastructure.Repositories
{
    public class AuthorRepository : BaseRepository<Author>
    {
        private IMapper mapper;

        public AuthorRepository(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext)
        {
            this.mapper = mapper;
        }

        public override void Update(Author existingAuthor, Author author)
        {
            mapper.Map(author, existingAuthor);
        }
    }
}
