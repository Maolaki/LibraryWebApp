using AutoMapper;
using LibraryWebApp.AuthorService.Domain.Entities;

namespace LibraryWebApp.AuthorService.Application.Profiles
{
    public class AuthorToAuthorProfile : Profile
    {
        public AuthorToAuthorProfile()
        {
            CreateMap<Author, Author>();
        }
    }
}
