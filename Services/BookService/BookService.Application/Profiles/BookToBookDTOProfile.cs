using AutoMapper;
using LibraryWebApp.BookService.Application.DTOs;
using LibraryWebApp.BookService.Domain.Entities;

namespace BookService.Application.Profiles
{
    public class BookToBookDTOProfile : Profile
    {
        public BookToBookDTOProfile() 
        {
            CreateMap<BookDTO, Book>().ReverseMap();
        }
    }
}
