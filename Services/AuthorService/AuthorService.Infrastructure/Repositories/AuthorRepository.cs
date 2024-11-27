using AutoMapper;
using LibraryWebApp.AuthorService.Domain.Entities;
using LibraryWebApp.AuthorService.Infrastructure.Context;

namespace LibraryWebApp.AuthorService.Infrastructure.Repositories
{
    public class AuthorRepository : BaseRepository<Author>
    {
        public AuthorRepository(ApplicationContext applicationContext, IMapper mapper) : base(applicationContext) {}
    }
}
