using AutoMapper;
using LibraryWebApp.AuthorService.Application.DTOs;
using LibraryWebApp.AuthorService.Domain.Entities;

namespace LibraryWebApp.AuthorService.Application.Profiles
{
    public class AuthorToAuthorDTOProfile : Profile
    {
        public AuthorToAuthorDTOProfile()
        {
            CreateMap<AuthorDTO, Author>().ReverseMap();
        }
    }
}
