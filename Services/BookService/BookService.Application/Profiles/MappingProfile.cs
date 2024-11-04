using AutoMapper;
using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Domain.Entities;

namespace LibraryWebApp.BookService.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, Book>();
            CreateMap<BookDTO, Book>().ReverseMap();
        }
    }
}
