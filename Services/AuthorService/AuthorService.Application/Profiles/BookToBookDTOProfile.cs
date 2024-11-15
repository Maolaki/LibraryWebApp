using AutoMapper;
using LibraryWebApp.AuthorService.Application.DTOs;
using LibraryWebApp.AuthorService.Domain.Entities;

namespace LibraryWebApp.AuthorService.Application.Profiles
{
    public class BookToBookDTOProfile : Profile
    {
        public BookToBookDTOProfile()
        {
            CreateMap<BookDTO, Book>().ReverseMap();
        }
    }
}
