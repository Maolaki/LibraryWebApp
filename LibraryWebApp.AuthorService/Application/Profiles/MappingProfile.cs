using AutoMapper;
using LibraryWebApp.AuthorService.Application.DTOs;
using LibraryWebApp.AuthorService.Domain.Entities;

namespace LibraryWebApp.AuthorService.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Author, Author>();
            CreateMap<AuthorDTO, Author>();
        }
    }
}
